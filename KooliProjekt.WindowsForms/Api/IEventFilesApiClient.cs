using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IEventFilesApiClient
    {
        Task<OperationResult<PagedResult<EventFile>>> List(int page, int pageSize);
        Task<OperationResult> Save(EventFile eventFile);
        Task<OperationResult> Delete(int id);
    }
}
