using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class ClientsApiClient : CrudApiClientBase<Client>, IClientsApiClient
    {
        public ClientsApiClient(HttpClient httpClient)
            : base(httpClient, "api/Clients/")
        {
        }

        protected override string GetListPath(int page, int pageSize)
        {
            return $"?page={page}&pageSize={pageSize}";
        }
    }
}

