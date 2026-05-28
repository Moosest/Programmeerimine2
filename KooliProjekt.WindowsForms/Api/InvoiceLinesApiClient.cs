using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class InvoiceLinesApiClient : CrudApiClientBase<InvoiceLine>, IInvoiceLinesApiClient
    {
        public InvoiceLinesApiClient(HttpClient httpClient)
            : base(httpClient, "api/InvoiceLines/")
        {
        }
    }
}
