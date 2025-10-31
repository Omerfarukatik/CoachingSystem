// CoachingSystem.API/Models/DTOs/Auth/AuthResultDto.cs

namespace CoachingSystem.API.Models.DTOs.Auth;

// Yanıt içinde sadece gerekli kullanıcı bilgilerini taşıyan basit bir DTO
public class AuthUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; 
}

// Ana Yanıt DTO'su
public class AuthResultDto
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new List<string>(); 
    
    // Token ve User sadece Success: true olduğunda dolacak.
    public string? Token { get; set; }
    public AuthUserDto? User { get; set; }
}