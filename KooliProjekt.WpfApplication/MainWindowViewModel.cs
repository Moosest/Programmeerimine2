using System.Collections.ObjectModel;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IClientsApiClient _clientsApiClient;
        private readonly ObservableCollection<Client> _data;
        private Client? _selectedItem;

        public MainWindowViewModel()
            : this(new ClientsApiClient(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            }))
        {
        }

        public MainWindowViewModel(IClientsApiClient clientsApiClient)
        {
            _clientsApiClient = clientsApiClient;
            _data = new ObservableCollection<Client>();
        }

        public async Task<OperationResult> LoadDataAsync()
        {
            var response = await _clientsApiClient.List(1, 10);
            if (response?.HasErrors == true)
            {
                return response;
            }

            _data.Clear();

            if (response?.Value?.Results != null)
            {
                foreach (var item in response.Value.Results)
                {
                    _data.Add(item);
                }
            }

            return new OperationResult();
        }

        public ObservableCollection<Client> Data
        {
            get
            {
                return _data;
            }
        }

        public Client? SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }
    }
}
