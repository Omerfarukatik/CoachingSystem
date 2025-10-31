// CoachingSystem.API/Controllers/AuthController.cs

using CoachingSystem.Application.Interfaces;
using CoachingSystem.Domain.Entities;
using CoachingSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using CoachingSystem.API.Models.DTOs.Auth; // ✨ Yeni DTO'lar için
using System.Linq; // LINQ için
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations; // Validasyon için (Genellikle buna gerek kalmaz, ama burada ekleyelim)

namespace CoachingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous] // ✨ ÖNEMLİ: Bu controller'daki tüm endpoint'leri global [Authorize] filtresinden muaf tutar.
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    // ✨ Giriş DTO'su kullanılıyor
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        // Model validasyonu (DataAnnotations) başarısız olursa
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new AuthResultDto { Success = false, Errors = errors });
        }

        // E-posta kontrolü
        if (await _userRepository.GetByEmailAsync(request.Email) != null)
        {
            // ✨ Hata yanıtı standart DTO ile döndürülüyor
            return BadRequest(new AuthResultDto 
            { 
                Success = false, 
                Errors = new List<string> { "Bu e-posta adresi zaten kullanımda." } 
            });
        }
        
        // 1. Şifreyi hash'le
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // 2. Yeni kullanıcıyı oluştur
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = request.Role, // Coach, Client vb.
            PasswordHash = hashedPassword 
        };

        await _userRepository.AddAsync(user);

        // 3. Token üret ve gönder (Rol bilgisi TokenService içinde JWT'ye eklenecek)
        // **ÖNEMLİ:** ITokenService/CreateToken metodunuz, bu kullanıcının rolünü (user.Role) token'a Claim olarak eklemelidir.
        var token = _tokenService.CreateToken(user); 

        // ✨ Başarılı yanıt standart DTO ile döndürülüyor
        return Ok(new AuthResultDto
        {
            Success = true,
            Token = token,
            User = new AuthUserDto { Id = user.Id, Email = user.Email, Role = user.Role.ToString() }
        });
    }
    
    [HttpPost("login")]
    // ✨ Giriş DTO'su kullanılıyor
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        // Kullanıcı yok VEYA şifre yanlışsa
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            // ✨ Güvenlik için spesifik hata vermekten kaçınılır.
            return Unauthorized(new AuthResultDto
            {
                Success = false,
                Errors = new List<string> { "E-posta veya şifre geçersiz." }
            });
        }

        // Başarılı ise token üret (Rol bilgisi ITokenService içinde JWT'ye eklenmeli)
        var token = _tokenService.CreateToken(user);

        // ✨ Başarılı yanıt standart DTO ile döndürülüyor
        return Ok(new AuthResultDto
        {
            Success = true,
            Token = token,
            User = new AuthUserDto { Id = user.Id, Email = user.Email, Role = user.Role.ToString() }
        });
    }
}