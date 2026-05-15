using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface ISystemUsersApiClient
    {
        Task<OperationResult<PagedResult<SystemUser>>> List(int page, int pageSize);
        Task<OperationResult> Save(SystemUser systemUser);
        Task<OperationResult> Delete(int id);
    }
}
