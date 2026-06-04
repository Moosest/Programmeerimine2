using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class PaymentsApiClient : CrudApiClientBase<Payment>, IPaymentsApiClient
    {
        public PaymentsApiClient(HttpClient httpClient)
            : base(httpClient, "api/Payments/")
        {
        }
    }
}

