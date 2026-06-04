using System.Windows;
using System.Windows.Controls;

namespace KooliProjekt.WpfApplication
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            };

            var clientsApiClient = new ClientsApiClient(httpClient);
            _viewModel = new MainWindowViewModel(clientsApiClient);
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var result = await _viewModel.LoadData();

            if (result.HasErrors)
            {
                var errors = result.Errors == null ? string.Empty : string.Join(Environment.NewLine, result.Errors);
                MessageBox.Show(errors, "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            clientsGrid.ItemsSource = _viewModel.DataSource;

            if (_viewModel.DataSource.Count > 0)
            {
                clientsGrid.SelectedIndex = 0;
            }
            else
            {
                _viewModel.SetSelection(null);
                SyncTextBoxes();
            }
        }

        private void clientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedClient = clientsGrid.SelectedItem as Client;
            _viewModel.SetSelection(selectedClient);
            SyncTextBoxes();
        }

        private void SyncTextBoxes()
        {
            idTextBox.Text = _viewModel.CurrentId.ToString();
            nameTextBox.Text = _viewModel.CurrentName;
            emailTextBox.Text = _viewModel.CurrentEmail;
            phoneTextBox.Text = _viewModel.CurrentPhone;
            addressTextBox.Text = _viewModel.CurrentAddress;
            discountTextBox.Text = _viewModel.CurrentDiscount;
        }
    }
}