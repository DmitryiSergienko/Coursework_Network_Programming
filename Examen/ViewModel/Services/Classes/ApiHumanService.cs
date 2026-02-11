#nullable enable
using System.Net.Http;
using System.Text.Json;
using ViewModel.Models;

namespace ViewModel.Services.Classes;

public class ApiHumanService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5000";

    public ApiHumanService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<HumanDto?> GetHumanByIdAsync(int id, string humanType)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/{humanType}s/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<HumanDto>(json);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteHumanAsync(int id, string humanType)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/{humanType}s/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}