// CoachingSystem.Domain/Services/IPasswordHasher.cs

namespace CoachingSystem.Domain.Services;

// Domain katmanında, şifre işleme için bir kontrat (arayüz) tanımlıyoruz.
// Bu kontratın somut uygulamasını Infrastructure katmanında (BCrypt ile) yapacağız.
public interface IPasswordHasher
{
    // Şifreyi hash'ler
    byte[] HashPassword(string password);
    
    // Girilen şifrenin, saklanan hash ile eşleşip eşleşmediğini kontrol eder
    bool VerifyPassword(string password, byte[] storedHash);
}