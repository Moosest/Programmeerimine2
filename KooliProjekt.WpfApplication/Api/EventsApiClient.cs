using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class EventsApiClient : CrudApiClientBase<Event>, IEventsApiClient
    {
        public EventsApiClient(HttpClient httpClient)
            : base(httpClient, "api/Events/")
        {
        }
    }
}

