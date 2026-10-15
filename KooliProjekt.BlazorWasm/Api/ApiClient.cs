using System.Net.Http.Json;
using Newtonsoft.Json;

namespace KooliProjekt.BlazorWasm
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Clients/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<Client>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<Client>>>(body);
            return result;
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

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete?id=" + id;

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }
    }
}
