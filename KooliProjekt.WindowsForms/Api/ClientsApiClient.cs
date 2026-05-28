using System.Net.Http.Json;
using System.Text.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class ClientsApiClient : IClientsApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ClientsApiClient(HttpClient httpClient)
        {
            _baseUrl = "api/Clients/";
            _client = httpClient;
        }

        public async Task<OperationResult<PagedResult<Client>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(body))
            {
                return new OperationResult<PagedResult<Client>>()
                    .AddError($"Request failed: {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            var result = JsonSerializer.Deserialize<OperationResult<PagedResult<Client>>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult<PagedResult<Client>>();
        }

        public async Task<OperationResult> Save(Client client)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(client)
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
