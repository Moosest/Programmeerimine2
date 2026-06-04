using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class InvoiceLinesApiClient : CrudApiClientBase<InvoiceLine>, IInvoiceLinesApiClient
    {
        public InvoiceLinesApiClient(HttpClient httpClient)
            : base(httpClient, "api/InvoiceLines/")
        {
        }
    }
}

