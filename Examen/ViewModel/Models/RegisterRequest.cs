#nullable enable
namespace ViewModel.Models;

public class RegisterRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public string Mail { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string HumanType { get; set; } = "user"; // "user" или "admin"
}