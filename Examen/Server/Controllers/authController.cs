using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Model;
using Server.Dtos;

namespace Server.Controllers
{
    // Это базовый атрибут для всех контроллеров Web API
    [ApiController]
    // Это путь: "api/auth" — по этому адресу будет доступен контроллер
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly Service _service;

        // Конструктор — сюда ASP.NET автоматически передаст твой Service
        public authController(Service service)
        {
            _service = service;
        }

        // Это метод API для входа
        // Когда клиент вызовет POST запрос на "api/auth/login" — сработает этот метод
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Создаём модели для проверки
            var user = new UsersModel(request.Login, request.Password);
            var admin = new AdminsModel(request.Login, request.Password);

            // 2. Проверяем авторизацию
            if (await _service.OnLoginAsync(user))
            {
                if (_service.CurrentHuman == null)
                    return Unauthorized(new { error = "Ошибка авторизации" });

                // 3. Формируем БЕЗОПАСНЫЙ ответ без пароля
                var response = new LoginResponse
                {
                    Id = _service.CurrentHuman.Id,
                    Login = _service.CurrentHuman.Login,
                    Name = _service.CurrentHuman.Name,
                    Surname = _service.CurrentHuman.Surname,
                    Type = "user"
                };
                return Ok(response);
            }
            else if (await _service.OnLoginAsync(admin))
            {
                if (_service.CurrentHuman == null)
                    return Unauthorized(new { error = "Ошибка авторизации" });

                var response = new LoginResponse
                {
                    Id = _service.CurrentHuman.Id,
                    Login = _service.CurrentHuman.Login,
                    Name = _service.CurrentHuman.Name,
                    Surname = _service.CurrentHuman.Surname,
                    Type = "admin"
                };
                return Ok(response);
            }

            return Unauthorized(new { error = "Неверный логин или пароль" });
        }
    }

    // Модель данных для запроса входа
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}