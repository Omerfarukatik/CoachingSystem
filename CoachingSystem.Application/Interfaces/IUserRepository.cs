// CoachingSystem.Application/Interfaces/IUserRepository.cs

using CoachingSystem.Domain.Entities;

namespace CoachingSystem.Application.Interfaces;

// Veri erişimi için kontrat (sözleşme)
public interface IUserRepository
{
    // ID ile kullanıcıyı getir
    Task<User?> GetByIdAsync(Guid id);

    // Yeni bir kullanıcı ekle
    Task<User> AddAsync(User user);
    
    Task<User?> GetByEmailAsync(string email);
    // İleride buraya GetAll, Update vb. metotlar eklenecek.
}