using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class EventFilesApiClient : CrudApiClientBase<EventFile>, IEventFilesApiClient
    {
        public EventFilesApiClient(HttpClient httpClient)
            : base(httpClient, "api/EventFiles/")
        {
        }
    }
}

