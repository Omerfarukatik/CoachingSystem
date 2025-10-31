// CoachingSystem.Application/Interfaces/ITokenService.cs

using CoachingSystem.Domain.Entities;

namespace CoachingSystem.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}