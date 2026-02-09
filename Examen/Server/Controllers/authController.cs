#nullable enable
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Server.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Service _service;

    public AuthController(Service service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Логин и пароль обязательны" });
        }

        try
        {
            var user = new UsersModel(request.Login, request.Password);
            var admin = new AdminsModel(request.Login, request.Password);

            if (await _service.OnLoginAsync(user))
            {
                var currentHuman = _service.CurrentHuman
                    ?? throw new InvalidOperationException("CurrentHuman is null after successful login");

                return Ok(new LoginResponse
                {
                    Id = currentHuman.Id,
                    Login = currentHuman.Login,
                    Name = currentHuman.Name ?? string.Empty,
                    Surname = currentHuman.Surname ?? string.Empty,
                    Type = "user"
                });
            }
            else if (await _service.OnLoginAsync(admin))
            {
                var currentHuman = _service.CurrentHuman
                    ?? throw new InvalidOperationException("CurrentHuman is null after successful login");

                return Ok(new LoginResponse
                {
                    Id = currentHuman.Id,
                    Login = currentHuman.Login,
                    Name = currentHuman.Name ?? string.Empty,
                    Surname = currentHuman.Surname ?? string.Empty,
                    Type = "admin"
                });
            }

            return Unauthorized(new { error = "Неверный логин или пароль" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _service.LogOut();
        return Ok(new { message = "Выход выполнен" });
    }
}