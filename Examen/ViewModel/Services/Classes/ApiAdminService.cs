using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ViewModel.Services
{
    public class ApiAdminService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = "http://localhost:5191";

        public async Task<DataTable> ExecuteSqlQueryAsync(string query)
        {
            try
            {
                var request = new { sqlQuery = query };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/admin/execute-query", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    // Здесь должна быть логика преобразования JSON в DataTable
                    // Для простоты возвращаем пустую таблицу
                    return new DataTable();
                }
                else
                {
                    var errorTable = new DataTable();
                    errorTable.Columns.Add("Ошибка", typeof(string));
                    errorTable.Rows.Add($"HTTP ошибка: {response.StatusCode}");
                    return errorTable;
                }
            }
            catch (Exception ex)
            {
                var errorTable = new DataTable();
                errorTable.Columns.Add("Ошибка", typeof(string));
                errorTable.Rows.Add(ex.Message);
                return errorTable;
            }
        }
    }
}