using System.Data;
using System.Net.Http;

namespace ViewModel.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = "http://localhost:5191";

        public async Task<DataTable> GetTop3ProductsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/products/top3");
            return new DataTable();
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