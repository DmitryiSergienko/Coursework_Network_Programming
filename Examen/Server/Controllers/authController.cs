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
            Console.WriteLine("✅ Запрос получен. Порт: " + request.Login);
            Console.WriteLine($"[CONTROLLER] Получен запрос: login={request.Login}, password={request.Password}");

            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Логин и пароль обязательны" });
            }
            try
            {
                // 1. Создаём модели для проверки
                var user = new UsersModel(request.Login, request.Password);
                var admin = new AdminsModel(request.Login, request.Password);

                // 2. Проверяем авторизацию
                Console.WriteLine("[CONTROLLER] Пытаюсь войти");
                Console.WriteLine($"[CONTROLLER] CurrentHuman: {_service.CurrentHuman?.Login ?? "NULL"}");
                if (await _service.OnLoginAsync(user))
                {
                    Console.WriteLine("[CONTROLLER] Успешный вход как User");
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
                    Console.WriteLine("[CONTROLLER] Успешный вход как Admin");
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

                Console.WriteLine("[DEBUG] Авторизация провалена для: " + request.Login);
                return Unauthorized(new { error = "Неверный логин или пароль" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _service.LogOut();
            return Ok(new { message = "Выход выполнен" });
        }
    }

    // Модель данных для запроса входа
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}