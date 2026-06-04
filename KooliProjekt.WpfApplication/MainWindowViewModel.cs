namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel
    {
        private readonly IClientsApiClient _clientsApiClient;

        public IList<Client> DataSource { get; private set; }
        public Client? SelectedItem { get; private set; }

        public int CurrentId { get; private set; }
        public string CurrentName { get; private set; }
        public string CurrentEmail { get; private set; }
        public string CurrentPhone { get; private set; }
        public string CurrentAddress { get; private set; }
        public string CurrentDiscount { get; private set; }

        public MainWindowViewModel(IClientsApiClient clientsApiClient)
        {
            _clientsApiClient = clientsApiClient;
            DataSource = new List<Client>();
            CurrentName = string.Empty;
            CurrentEmail = string.Empty;
            CurrentPhone = string.Empty;
            CurrentAddress = string.Empty;
            CurrentDiscount = string.Empty;
        }

        public async Task<OperationResult> LoadData()
        {
            var response = await _clientsApiClient.List(1, 10);
            if (response?.HasErrors == true)
            {
                return response;
            }

            DataSource = response?.Value?.Results ?? new List<Client>();
            return new OperationResult();
        }

        public void SetSelection(Client? selectedClient)
        {
            SelectedItem = selectedClient;

            if (selectedClient == null)
            {
                CurrentId = 0;
                CurrentName = string.Empty;
                CurrentEmail = string.Empty;
                CurrentPhone = string.Empty;
                CurrentAddress = string.Empty;
                CurrentDiscount = string.Empty;
                return;
            }

            CurrentId = selectedClient.Id;
            CurrentName = selectedClient.Name;
            CurrentEmail = selectedClient.Email;
            CurrentPhone = selectedClient.Phone;
            CurrentAddress = selectedClient.Address;
            CurrentDiscount = selectedClient.Discount.ToString();
        }
    }
}
