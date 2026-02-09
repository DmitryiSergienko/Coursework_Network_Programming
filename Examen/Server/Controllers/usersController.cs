using Microsoft.AspNetCore.Mvc;
using DataLayer.Services;
using Server.Dtos;
using Model;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Service _service;

        public UsersController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _service.GetListAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении пользователей: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _service.GetHumanInfoById(id, "user");
                if (user == null)
                    return NotFound($"Пользователь с ID {id} не найден");

                var dto = new UserDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name ?? string.Empty,
                    Surname = user.Surname ?? string.Empty,
                    Patronymic = user.Patronymic ?? string.Empty,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    RegistrationDate = user.Registration_date
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = new UsersModel(
                    request.Login,
                    request.Password,
                    request.Name,
                    request.Surname,
                    request.Patronymic,
                    request.Mail,
                    request.PhoneNumber
                );

                int userId = await _service.AddHumanAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { Id = userId, Message = "Пользователь создан" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка регистрации: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.Context_Procedures.Procedures.stp_user_updateAsync(
                    id: id,
                    login: request.Login,
                    password: request.Password,
                    name: request.Name,
                    surname: request.Surname,
                    patronymic: request.Patronymic,
                    mail: request.Mail,
                    phone_number: request.PhoneNumber
                );

                return Ok(new { Message = "Данные пользователя обновлены" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка обновления: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _service.Context_Procedures.Procedures.stp_user_deleteAsync(id);
                return Ok(new { Message = "Пользователь удалён" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка удаления: {ex.Message}");
            }
        }
    }

    public class RegisterUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}