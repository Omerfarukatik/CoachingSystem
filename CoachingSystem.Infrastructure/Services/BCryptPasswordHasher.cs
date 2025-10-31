// CoachingSystem.Infrastructure/Services/BCryptPasswordHasher.cs

using CoachingSystem.Domain.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CoachingSystem.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public byte[] HashPassword(string password)
    {
        string hash = BCryptNet.HashPassword(password);
        return System.Text.Encoding.UTF8.GetBytes(hash);
    }

    public bool VerifyPassword(string password, byte[] storedHash)
    {
        string hashString = System.Text.Encoding.UTF8.GetString(storedHash);
        return BCryptNet.Verify(password, hashString);
    }
}