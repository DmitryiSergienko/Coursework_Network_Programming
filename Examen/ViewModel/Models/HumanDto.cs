#nullable enable
namespace ViewModel.Models;

public class HumanDto
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public string Mail { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}