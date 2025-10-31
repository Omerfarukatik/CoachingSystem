// CoachingSystem.Infrastructure/Data/ApplicationDbContext.cs

using CoachingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachingSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domain'deki User varlığımızı veritabanındaki "Users" tablosuna eşlemek için
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL'de C# Enum'larını kullanmak için gerekli ayarlama (Npgsql kütüphanesinden)
        modelBuilder.HasPostgresEnum<UserRole>();

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique(); // Email benzersiz olmalı
        });

        base.OnModelCreating(modelBuilder);
    }
}
/*### 🛠️ Adım 3: Migrasyon ve Veritabanını Güncelleme

Bu yeni tabloyu veritabanınıza uygulamak için terminalde (ana `CoachingSystem` klasöründeyken) şu komutları çalıştırın:

1.  **Yeni Migrasyon Oluşturma:**
    ```bash
    dotnet ef migrations add AddSessionEntity --project CoachingSystem.Infrastructure --startup-project CoachingSystem.API
    ```
2.  **Veritabanını Güncelleme:**
    ```bash
    dotnet ef database update --startup-project CoachingSystem.API*/
    
