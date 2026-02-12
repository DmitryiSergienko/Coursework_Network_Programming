#nullable enable
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ViewModel.Models;

namespace ViewModel.Services.Classes
{
    public class ApiAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5000";

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

            var url = $"{_baseUrl}/api/Auth/login";

            System.Diagnostics.Debug.WriteLine("==========================================");
            System.Diagnostics.Debug.WriteLine($"[API] URL: {url}");
            System.Diagnostics.Debug.WriteLine($"[API] Request: {json}");
            System.Diagnostics.Debug.WriteLine("==========================================");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var responseJson = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"[API] Status: {(int)response.StatusCode} {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"[API] Response: {responseJson}");
                System.Diagnostics.Debug.WriteLine("==========================================");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return JsonSerializer.Deserialize<LoginResponse>(responseJson, options);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] EXCEPTION: {ex.Message}");
                return null;
            }
        }
    }
}