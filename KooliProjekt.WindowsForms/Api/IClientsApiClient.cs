using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IClientsApiClient
    {
        Task<OperationResult<PagedResult<Client>>> List(int page, int pageSize);
        Task<OperationResult> Save(Client client);
        Task<OperationResult> Delete(int id);
    }
}
