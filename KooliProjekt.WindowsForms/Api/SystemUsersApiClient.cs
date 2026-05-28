using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class SystemUsersApiClient : CrudApiClientBase<SystemUser>, ISystemUsersApiClient
    {
        public SystemUsersApiClient(HttpClient httpClient)
            : base(httpClient, "api/SystemUsers/")
        {
        }
    }
}
