using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class InvoicesApiClient : CrudApiClientBase<Invoice>, IInvoicesApiClient
    {
        public InvoicesApiClient(HttpClient httpClient)
            : base(httpClient, "api/Invoices/")
        {
        }
    }
}
