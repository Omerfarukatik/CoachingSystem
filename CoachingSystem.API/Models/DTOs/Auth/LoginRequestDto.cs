// CoachingSystem.API/Models/DTOs/Auth/LoginRequestDto.cs

using System.ComponentModel.DataAnnotations;

namespace CoachingSystem.API.Models.DTOs.Auth;

public record LoginRequestDto(
    [Required][EmailAddress] string Email, 
    [Required] string Password
);