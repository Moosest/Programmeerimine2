using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IPaymentsApiClient
    {
        Task<OperationResult<PagedResult<Payment>>> List(int page, int pageSize);
        Task<OperationResult> Save(Payment payment);
        Task<OperationResult> Delete(int id);
    }
}
