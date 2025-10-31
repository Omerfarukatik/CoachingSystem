// CoachingSystem.Domain/Entities/User.cs

namespace CoachingSystem.Domain.Entities;

public enum UserRole{
    Client, // Koçluk Alan
    Coach,  // Koç
    Manager,
    Admin
}

public class User
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public byte[] PasswordHash { get; set; } = new byte[0]; // Şifre hash'i
}