// CoachingSystem.API/Controllers/UsersController.cs

using CoachingSystem.Application.Interfaces;
using CoachingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoachingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    // IUserRepository'yi Dependency Injection ile alıyoruz.
    // Controller, hangi Repository implementasyonunun (PostgreSQL) kullanıldığını bilmez, sadece arayüzü bilir.
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/Users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}