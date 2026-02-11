using System.Data;
using System.Net.Http;
using System.Text.Json;

namespace ViewModel.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = "http://localhost:5000";

        public async Task<DataTable> GetTop3ProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/products/top3");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonToDataTable(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return new DataTable();
        }

        // Вспомогательный метод для преобразования JSON в DataTable
        private DataTable JsonToDataTable(string json)
        {
            var table = new DataTable();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in root.EnumerateArray())
                {
                    if (table.Columns.Count == 0)
                    {
                        foreach (var prop in item.EnumerateObject())
                        {
                            table.Columns.Add(prop.Name, typeof(string));
                        }
                    }

                    var row = table.NewRow();
                    foreach (var prop in item.EnumerateObject())
                    {
                        row[prop.Name] = prop.Value.ToString();
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public async Task<DataTable> GetShowProductsInPortionsAsync(int skipRows, int count)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/products/portions?skip={skipRows}&count={count}");
            return new DataTable();
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/products/count");
            return 0;
        }

        public async Task<DataTable> GetUserOrderHistoryAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/orders/history");
            return new DataTable();
        }
    }
}