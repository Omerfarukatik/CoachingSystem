// CoachingSystem.Application/Services/ITokenService.cs

using CoachingSystem.Domain.Entities;

namespace CoachingSystem.Application.Services;

public interface ITokenService
{
    // Verilen kullanıcı için JWT Token üretir
    string CreateToken(User user);
}