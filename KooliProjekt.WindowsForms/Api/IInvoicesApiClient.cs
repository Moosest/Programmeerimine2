using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IInvoicesApiClient
    {
        Task<OperationResult<PagedResult<Invoice>>> List(int page, int pageSize);
        Task<OperationResult> Save(Invoice invoice);
        Task<OperationResult> Delete(int id);
    }
}
