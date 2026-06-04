using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class InvoicesApiClient : CrudApiClientBase<Invoice>, IInvoicesApiClient
    {
        public InvoicesApiClient(HttpClient httpClient)
            : base(httpClient, "api/Invoices/")
        {
        }
    }
}

