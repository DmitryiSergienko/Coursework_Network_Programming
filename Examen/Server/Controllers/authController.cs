using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Server.DTOs;

namespace Server.Controllers
{
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
            System.Diagnostics.Debug.WriteLine($"[SERVER] ========== НОВЫЙ ЗАПРОС ==========");
            System.Diagnostics.Debug.WriteLine($"[SERVER] Login: {request.Login}");
            System.Diagnostics.Debug.WriteLine($"[SERVER] Password: {request.Password}");

            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Логин и пароль обязательны" });
            }

            try
            {
                // 1. СНАЧАЛА ПРОВЕРЯЕМ АДМИНА
                System.Diagnostics.Debug.WriteLine($"[SERVER] Проверяем админа...");
                var admin = new AdminsModel(request.Login, request.Password);
                var isAdmin = await _service.OnLoginAsync(admin);
                System.Diagnostics.Debug.WriteLine($"[SERVER] Результат проверки админа: {isAdmin}");

                if (isAdmin)
                {
                    var currentHuman = _service.CurrentHuman;
                    System.Diagnostics.Debug.WriteLine($"[SERVER] АДМИН НАЙДЕН! ID: {currentHuman?.Id}, Login: {currentHuman?.Login}");

                    if (currentHuman == null)
                    {
                        return StatusCode(500, new { error = "CurrentHuman is null after successful login" });
                    }

                    return Ok(new LoginResponse
                    {
                        Id = currentHuman.Id,
                        Login = currentHuman.Login,
                        Name = currentHuman.Name ?? string.Empty,
                        Surname = currentHuman.Surname ?? string.Empty,
                        Type = "admin"
                    });
                }

                // 2. ПОТОМ ПРОВЕРЯЕМ ЮЗЕРА
                System.Diagnostics.Debug.WriteLine($"[SERVER] Проверяем юзера...");
                var user = new UsersModel(request.Login, request.Password);
                var isUser = await _service.OnLoginAsync(user);
                System.Diagnostics.Debug.WriteLine($"[SERVER] Результат проверки юзера: {isUser}");

                if (isUser)
                {
                    var currentHuman = _service.CurrentHuman;
                    System.Diagnostics.Debug.WriteLine($"[SERVER] ЮЗЕР НАЙДЕН! ID: {currentHuman?.Id}, Login: {currentHuman?.Login}");

                    if (currentHuman == null)
                    {
                        return StatusCode(500, new { error = "CurrentHuman is null after successful login" });
                    }

                    return Ok(new LoginResponse
                    {
                        Id = currentHuman.Id,
                        Login = currentHuman.Login,
                        Name = currentHuman.Name ?? string.Empty,
                        Surname = currentHuman.Surname ?? string.Empty,
                        Type = "user"
                    });
                }

                System.Diagnostics.Debug.WriteLine($"[SERVER] НЕ НАЙДЕН!");
                return Unauthorized(new { error = "Неверный логин или пароль" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SERVER] ОШИБКА: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}