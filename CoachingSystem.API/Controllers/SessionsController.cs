using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoachingSystem.API.Controllers;

[Route("api/Sessions")] // Route name fixed to avoid conflicts
[ApiController]
public class SessionsController : ControllerBase
{
    // Constructor ve DI (Dependency Injection) burada yer alacak
    // private readonly ISessionService _sessionService; 

    public SessionsController(/* ISessionService sessionService */)
    {
        // _sessionService = sessionService;
    }

    // GET /api/sessions/all
    // Gerekli Rol: Sadece Coach, Mudur ve Admin tüm seansları listeleyebilir.
    [HttpGet("all")]
    // ✨ DÜZELTME: Rol isimleri UserRole enum'ı ile eşleşmelidir (Coach, Manager, Admin).
    [Authorize(Roles = "Coach,Manager,Admin")] 
    public IActionResult GetAllSessions()
    {
        // ... (Veritabanından tüm seansları çekme mantığı) ...
        return Ok(new { Message = "Sistemdeki tüm seanslar başarıyla listelendi." });
    }

    // GET /api/sessions/{id}
    // Gerekli Rol: Giriş yapmış herkes kendi seans detayını görebilir (Bu metot çalışıyordu).
    [HttpGet("{id}")]
    [Authorize] 
    public IActionResult GetSessionDetails(Guid id)
    {
        // ... (Seans detayını çekme ve kullanıcının bu seansa erişim yetkisini kontrol etme) ...
        return Ok(new { Message = $"Seans ID: {id} detayı yüklendi." });
    }
    
    // POST /api/sessions
    // Gerekli Rol: Sadece Koçlar seans oluşturabilir.
    // ✨ GÜNCELLEME: Admin rolü de artık seans oluşturabilir.
    [HttpPost]
    [Authorize(Roles = "Coach,Admin")] 
    public IActionResult CreateSession([FromBody] object createSessionDto)
    {
        // ... (Seans oluşturma mantığı) ...
        return Ok(new { Message = "Yeni seans başarıyla oluşturuldu." });
    }

    // PUT /api/sessions/{id}
    // Gerekli Rol: Coach, Manager veya Admin bir seansı güncelleyebilir.
    [HttpPut("{id}")]
    [Authorize(Roles = "Coach,Manager,Admin")]
    public IActionResult UpdateSession(Guid id, [FromBody] object updateSessionDto)
    {
        // ... (Güncelleme mantığı) ...
        // Örnek: Önce seansı bul, sonra güncellemeyi uygula.
        return Ok(new { Message = $"Seans ID: {id} başarıyla güncellendi." });
    }

    // GET /api/sessions/my-sessions
    // Gerekli Rol: Sadece Client kendi seanslarını listeleyebilir.
    [HttpGet("my-sessions")]
    [Authorize(Roles = "Client")]
    public IActionResult GetMySessions()
    {
        // ... (Giriş yapmış kullanıcının ID'sini token'dan alıp sadece ona ait seansları listeleme) ...
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { Message = "Size ait seanslar başarıyla listelendi." });
    }

    // DELETE /api/sessions/{id}
    // Gerekli Rol: Sadece Müdür veya Admin bir seansı silebilir.
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Admin")] 
    public IActionResult DeleteSession(Guid id)
    {
        // ... (Silme işlemi) ...
        return Ok(new { Message = $"Seans ID: {id} başarıyla silindi." });
    }

    // POST /api/sessions/{id}/archive
    // Gerekli Rol: Sadece Manager veya Admin bir seansı arşivleyebilir.
    [HttpPost("{id}/archive")]
    [Authorize(Roles = "Manager,Admin")]
    public IActionResult ArchiveSession(Guid id)
    {
        // ... (Seansın durumunu "Arşivlendi" olarak değiştirme mantığı) ...
        return Ok(new { Message = $"Seans ID: {id} başarıyla arşivlendi." });
    }
}
