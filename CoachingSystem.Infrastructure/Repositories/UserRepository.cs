// CoachingSystem.Infrastructure/Repositories/UserRepository.cs

using CoachingSystem.Application.Interfaces;
using CoachingSystem.Domain.Entities;
using CoachingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoachingSystem.Infrastructure.Repositories;

// Application'daki IUserRepository arayüzünü (kontratı) uyguluyoruz
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    // DbContext'i Dependency Injection ile alıyoruz
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user); // Kullanıcıyı belleğe ekle
        await _context.SaveChangesAsync();    // Değişiklikleri veritabanına kaydet
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }
}