using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IEventSchedulesApiClient
    {
        Task<OperationResult<PagedResult<EventSchedule>>> List(int page, int pageSize);
        Task<OperationResult> Save(EventSchedule eventSchedule);
        Task<OperationResult> Delete(int id);
    }
}
