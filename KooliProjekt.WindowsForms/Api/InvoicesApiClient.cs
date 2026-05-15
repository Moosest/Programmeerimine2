using System.Net.Http.Json;
using System.Text.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class InvoicesApiClient : IInvoicesApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public InvoicesApiClient(HttpClient httpClient)
        {
            _baseUrl = "api/Invoices/";
            _client = httpClient;
        }

        public async Task<OperationResult<PagedResult<Invoice>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OperationResult<PagedResult<Invoice>>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult<PagedResult<Invoice>>();
        }

        public async Task<OperationResult> Save(Invoice invoice)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(invoice)
            };
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OperationResult>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult();
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete?id=" + id;

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OperationResult>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult();
        }
    }
}
