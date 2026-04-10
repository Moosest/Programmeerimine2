using System.Text.Json;
using System.Net.Http.Json;

namespace KooliProjekt.WindowsForms
{
    public class ClientsApiClient : IClientsApiClient
    {
        private readonly HttpClient _httpClient;

        public ClientsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationResult<PagedResult<Client>>> List(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"api/Clients?page={page}&pageSize={pageSize}");
            return await Deserialize<OperationResult<PagedResult<Client>>>(response)
                ?? new OperationResult<PagedResult<Client>>();
        }

        public async Task<OperationResult> Save(Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Clients/Save", client);
            return await Deserialize<OperationResult>(response)
                ?? new OperationResult();
        }

        public async Task<OperationResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Clients/Delete?id={id}");
            return await Deserialize<OperationResult>(response)
                ?? new OperationResult();
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage response)
            where T : class
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<T>(content, options);
            if (result != null)
            {
                return result;
            }

            var innerJson = JsonSerializer.Deserialize<string>(content, options);
            if (string.IsNullOrWhiteSpace(innerJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(innerJson, options);
        }
    }
}
