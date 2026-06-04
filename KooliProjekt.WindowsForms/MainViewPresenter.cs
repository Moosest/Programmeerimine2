using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IClientsApiClient _clientsApiClient;
        private readonly IMainView _mainView;
        private Client _selectedClient;

        public MainViewPresenter(IClientsApiClient clientsApiClient, IMainView mainView)
        {
            _clientsApiClient = clientsApiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _clientsApiClient.List(1, 10);

            if (response?.HasErrors == true)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response?.Value?.Results;
        }

        public void AddNew()
        {
            SetSelection(null);
        }

        public void AddCommand_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        public async void SaveCommand_Click(object sender, EventArgs e)
        {
            await Save();
        }

        public async void DeleteCommand_Click(object sender, EventArgs e)
        {
            await Delete();
        }

        public async Task Save()
        {
            if (!decimal.TryParse(_mainView.CurrentDiscount, out var discount))
            {
                _mainView.ShowError("Viga salvestamisel", new OperationResult().AddError("Discount peab olema number."));
                return;
            }

            var client = new Client
            {
                Id = _mainView.CurrentId,
                Name = _mainView.CurrentName,
                Email = _mainView.CurrentEmail,
                Phone = _mainView.CurrentPhone,
                Address = _mainView.CurrentAddress,
                Discount = discount
            };

            var response = await _clientsApiClient.Save(client);
            if (response?.HasErrors == true)
            {
                _mainView.ShowError("Viga salvestamisel", response);
                return;
            }

            SetSelection(null);
            await LoadData();
        }

        public async Task Delete()
        {
            if (_selectedClient == null)
            {
                _mainView.ShowError("Viga kustutamisel", new OperationResult().AddError("Vali kustutatav rida."));
                return;
            }

            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var response = await _clientsApiClient.Delete(_selectedClient.Id);
            if (response?.HasErrors == true)
            {
                _mainView.ShowError("Viga kustutamisel", response);
                return;
            }

            SetSelection(null);
            await LoadData();
        }

        public void SetSelection(Client selectedClient)
        {
            _selectedClient = selectedClient;
            _mainView.SelectedItem = selectedClient;

            if (_selectedClient == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentName = string.Empty;
                _mainView.CurrentEmail = string.Empty;
                _mainView.CurrentPhone = string.Empty;
                _mainView.CurrentAddress = string.Empty;
                _mainView.CurrentDiscount = string.Empty;
                return;
            }

            _mainView.CurrentId = _selectedClient.Id;
            _mainView.CurrentName = _selectedClient.Name;
            _mainView.CurrentEmail = _selectedClient.Email;
            _mainView.CurrentPhone = _selectedClient.Phone;
            _mainView.CurrentAddress = _selectedClient.Address;
            _mainView.CurrentDiscount = _selectedClient.Discount.ToString();
        }
    }
}