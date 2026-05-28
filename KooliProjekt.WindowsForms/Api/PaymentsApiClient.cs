using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class PaymentsApiClient : CrudApiClientBase<Payment>, IPaymentsApiClient
    {
        public PaymentsApiClient(HttpClient httpClient)
            : base(httpClient, "api/Payments/")
        {
        }
    }
}
