using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IInvoiceLinesApiClient
    {
        Task<OperationResult<PagedResult<InvoiceLine>>> List(int page, int pageSize);
        Task<OperationResult> Save(InvoiceLine invoiceLine);
        Task<OperationResult> Delete(int id);
    }
}
