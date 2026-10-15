namespace KooliProjekt.BlazorWasm
{
    public interface IApiClient
    {
        Task<OperationResult<Client>> Get(int id);
        Task<OperationResult<PagedResult<Client>>> List(int page, int pageSize);
        Task<OperationResult> Save(Client client);
        Task<OperationResult> Delete(int id);
    }
}
