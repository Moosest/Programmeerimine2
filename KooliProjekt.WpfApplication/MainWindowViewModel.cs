using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IClientsApiClient _clientsApiClient;
        private readonly IDialogProvider _dialogProvider;
        private readonly ObservableCollection<Client> _data;
        private Client? _selectedItem;

        public ICommand AddNewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public MainWindowViewModel()
            : this(
                new ClientsApiClient(new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5086/")
                }),
                new DialogProvider())
        {
        }

        public MainWindowViewModel(IClientsApiClient clientsApiClient, IDialogProvider dialogProvider)
        {
            _clientsApiClient = clientsApiClient;
            _dialogProvider = dialogProvider;
            _data = new ObservableCollection<Client>();

            AddNewCommand = new RelayCommand<Client>(
                _ =>
                {
                    SelectedItem = new Client();
                });

            SaveCommand = new RelayCommand<Client>(
                async client =>
                {
                    if (client == null)
                    {
                        return;
                    }

                    var result = await _clientsApiClient.Save(client);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot save data", result);
                        return;
                    }

                    SelectedItem = null;
                    await LoadDataAsync();
                },
                _ => SelectedItem != null);

            DeleteCommand = new RelayCommand<Client>(
                async client =>
                {
                    if (client == null)
                    {
                        return;
                    }

                    var canDelete = _dialogProvider.Confirm("Are you sure you want to delete this item?");
                    if (!canDelete)
                    {
                        return;
                    }

                    var result = await _clientsApiClient.Delete(client.Id);
                    if (result.HasErrors)
                    {
                        ShowError("Cannot delete data", result);
                        return;
                    }

                    SelectedItem = null;
                    await LoadDataAsync();
                },
                _ => SelectedItem != null && SelectedItem.Id != 0);
        }

        public async Task LoadDataAsync()
        {
            var response = await _clientsApiClient.List(1, 10);
            if (response?.HasErrors == true)
            {
                ShowError("Cannot load data", response);
                return;
            }

            _data.Clear();

            if (response?.Value?.Results != null)
            {
                foreach (var item in response.Value.Results)
                {
                    _data.Add(item);
                }
            }
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
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = string.Empty;
            var propertyErrors = string.Empty;

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            _dialogProvider.ShowError(error.Trim());
        }
    }
}
