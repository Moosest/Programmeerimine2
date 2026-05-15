using System.Net.Http.Json;
using System.Text.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class EventSchedulesApiClient : IEventSchedulesApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public EventSchedulesApiClient(HttpClient httpClient)
        {
            _baseUrl = "api/EventSchedules/";
            _client = httpClient;
        }

        public async Task<OperationResult<PagedResult<EventSchedule>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OperationResult<PagedResult<EventSchedule>>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new OperationResult<PagedResult<EventSchedule>>();
        }

        public async Task<OperationResult> Save(EventSchedule eventSchedule)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(eventSchedule)
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
