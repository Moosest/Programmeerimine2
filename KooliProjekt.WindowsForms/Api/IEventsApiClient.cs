using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IEventsApiClient
    {
        Task<OperationResult<PagedResult<Event>>> List(int page, int pageSize);
        Task<OperationResult> Save(Event eventItem);
        Task<OperationResult> Delete(int id);
    }
}
