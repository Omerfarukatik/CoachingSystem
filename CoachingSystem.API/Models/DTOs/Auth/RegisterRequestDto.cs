// CoachingSystem.API/Models/DTOs/Auth/RegisterRequestDto.cs

using System.ComponentModel.DataAnnotations;
using CoachingSystem.Domain.Entities; // UserRole enum'u için

namespace CoachingSystem.API.Models.DTOs.Auth;

public record RegisterRequestDto(
    [Required] string FirstName, 
    [Required] string LastName, 
    [Required][EmailAddress] string Email, 
    [Required][MinLength(6)] string Password, 
    UserRole Role
);