#nullable enable
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ViewModel.Models;

namespace ViewModel.Services.Classes;

public class ApiAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5191"; // ← порт из launchSettings.json

    public ApiAuthService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<LoginResponse?> LoginAsync(string login, string password)
    {
        var request = new LoginRequest { Login = login, Password = password };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponse>(responseJson);
            }
            return null;
        }
        catch (Exception ex)
        {
            // Логирование ошибки (опционально)
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return null;
        }
    }
}