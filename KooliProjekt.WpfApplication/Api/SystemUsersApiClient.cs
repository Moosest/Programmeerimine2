using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class SystemUsersApiClient : CrudApiClientBase<SystemUser>, ISystemUsersApiClient
    {
        public SystemUsersApiClient(HttpClient httpClient)
            : base(httpClient, "api/SystemUsers/")
        {
        }
    }
}

