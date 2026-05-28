using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class EventFilesApiClient : CrudApiClientBase<EventFile>, IEventFilesApiClient
    {
        public EventFilesApiClient(HttpClient httpClient)
            : base(httpClient, "api/EventFiles/")
        {
        }
    }
}
