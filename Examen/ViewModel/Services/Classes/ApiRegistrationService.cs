#nullable enable
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ViewModel.Models;

namespace ViewModel.Services
{
    public class ApiRegistrationService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            // СЕРИАЛИЗУЕМ ЗАПРОС
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost:5000/api/users/register";

            // ============ ОТЛАДОЧНЫЙ ВЫВОД ============
            System.Diagnostics.Debug.WriteLine("==========================================");
            System.Diagnostics.Debug.WriteLine($"[REGISTER] URL: {url}");
            System.Diagnostics.Debug.WriteLine($"[REGISTER] Request Body: {json}");
            System.Diagnostics.Debug.WriteLine("==========================================");

            Console.WriteLine("==========================================");
            Console.WriteLine($"[REGISTER] URL: {url}");
            Console.WriteLine($"[REGISTER] Request Body: {json}");
            Console.WriteLine("==========================================");
            // ==========================================

            try
            {
                // ОТПРАВЛЯЕМ ЗАПРОС
                var response = await _httpClient.PostAsync(url, content);

                // ЧИТАЕМ ОТВЕТ
                var responseJson = await response.Content.ReadAsStringAsync();

                // ============ ОТЛАДОЧНЫЙ ВЫВОД ============
                System.Diagnostics.Debug.WriteLine($"[REGISTER] Status Code: {(int)response.StatusCode} {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"[REGISTER] Response Body: {responseJson}");
                System.Diagnostics.Debug.WriteLine("==========================================");

                Console.WriteLine($"[REGISTER] Status Code: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"[REGISTER] Response Body: {responseJson}");
                Console.WriteLine("==========================================");
                // ==========================================

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseJson);

                    // ПРОВЕРЯЕМ РАЗНЫЕ ВАРИАНТЫ НАПИСАНИЯ ID
                    if (doc.RootElement.TryGetProperty("id", out var idElement))
                        return idElement.GetInt32();
                    if (doc.RootElement.TryGetProperty("Id", out idElement))
                        return idElement.GetInt32();
                    if (doc.RootElement.TryGetProperty("ID", out idElement))
                        return idElement.GetInt32();
                    if (doc.RootElement.TryGetProperty("userId", out idElement))
                        return idElement.GetInt32();
                    if (doc.RootElement.TryGetProperty("UserId", out idElement))
                        return idElement.GetInt32();
                }

                return -1;
            }
            catch (Exception ex)
            {
                // ============ ОТЛАДОЧНЫЙ ВЫВОД ============
                System.Diagnostics.Debug.WriteLine($"[REGISTER] EXCEPTION: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[REGISTER] StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[REGISTER] EXCEPTION: {ex.Message}");
                Console.WriteLine($"[REGISTER] StackTrace: {ex.StackTrace}");
                // ==========================================

                return -1;
            }
        }
    }
}