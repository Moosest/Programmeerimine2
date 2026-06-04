using System.Text.Json;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApplication
{
    public abstract class CrudApiClientBase<TEntity>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly string _baseUrl;
        private readonly HttpClient _client;

        protected CrudApiClientBase(HttpClient httpClient, string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = httpClient;
        }

        public async Task<OperationResult<PagedResult<TEntity>>> List(int page, int pageSize)
        {
            var url = _baseUrl + GetListPath(page, pageSize);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = DeserializeResult<OperationResult<PagedResult<TEntity>>>(body);

            if (result != null)
            {
                return result;
            }

            if (!response.IsSuccessStatusCode)
            {
                return CreateErrorResult<PagedResult<TEntity>>(response, body);
            }

            return new OperationResult<PagedResult<TEntity>>();
        }

        public async Task<OperationResult> Save(TEntity entity)
        {
            var url = _baseUrl + GetSavePath();

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(entity)
            };
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = DeserializeResult<OperationResult>(body);

            if (result != null)
            {
                return result;
            }

            if (!response.IsSuccessStatusCode)
            {
                return CreateErrorResult(response, body);
            }

            return new OperationResult();
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + GetDeletePath(id);

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = DeserializeResult<OperationResult>(body);

            if (result != null)
            {
                return result;
            }

            if (!response.IsSuccessStatusCode)
            {
                return CreateErrorResult(response, body);
            }

            return new OperationResult();
        }

        protected virtual string GetListPath(int page, int pageSize) => $"List?page={page}&pageSize={pageSize}";

        protected virtual string GetSavePath() => "Save";

        protected virtual string GetDeletePath(int id) => $"Delete?id={id}";

        private static T? DeserializeResult<T>(string body) where T : class
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(body, _jsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static OperationResult CreateErrorResult(HttpResponseMessage response, string body)
        {
            var result = new OperationResult();
            AddApiErrors(result, response, body);
            return result;
        }

        private static OperationResult<T> CreateErrorResult<T>(HttpResponseMessage response, string body)
        {
            var result = new OperationResult<T>();
            AddApiErrors(result, response, body);
            return result;
        }

        private static void AddApiErrors(OperationResult result, HttpResponseMessage response, string body)
        {
            var problem = DeserializeResult<ApiProblemDetails>(body);

            if (problem?.Errors != null)
            {
                foreach (var error in problem.Errors)
                {
                    var message = error.Value == null || error.Value.Length == 0
                        ? "Validation error"
                        : string.Join(" ", error.Value);
                    result.AddPropertyError(error.Key, message);
                }
            }

            if (!string.IsNullOrWhiteSpace(problem?.Title))
            {
                result.AddError(problem.Title);
            }

            if (!string.IsNullOrWhiteSpace(problem?.Detail))
            {
                result.AddError(problem.Detail);
            }

            if (!result.HasErrors)
            {
                result.AddError($"Request failed: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }

        private class ApiProblemDetails
        {
            public string? Title { get; set; }
            public string? Detail { get; set; }
            public Dictionary<string, string[]>? Errors { get; set; }
        }
    }
}
