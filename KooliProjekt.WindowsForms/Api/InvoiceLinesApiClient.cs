using System.Net.Http.Json;
using System.Text.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class InvoiceLinesApiClient : IInvoiceLinesApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public InvoiceLinesApiClient(HttpClient httpClient)
        {
            _baseUrl = "api/InvoiceLines/";
            _client = httpClient;
        }

        public async Task<OperationResult<PagedResult<InvoiceLine>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OperationResult<PagedResult<InvoiceLine>>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult<PagedResult<InvoiceLine>>();
        }

        public async Task<OperationResult> Save(InvoiceLine invoiceLine)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(invoiceLine)
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
