using System.Net.Http;
using System.Text;
using System.Text.Json;
using ViewModel.Models;

namespace ViewModel.Services
{
    public class ApiRegistrationService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = "http://localhost:5191";

        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/register", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);
                if (doc.RootElement.TryGetProperty("Id", out var idElement))
                    return idElement.GetInt32();
            }
            return -1;
        }
    }
}