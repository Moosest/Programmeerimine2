using System.Windows;
namespace KooliProjekt.WpfApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            Loaded += async (s, e) =>
            {
                var result = await viewModel.LoadDataAsync();

                if (result.HasErrors)
                {
                    var errors = result.Errors == null ? string.Empty : string.Join(Environment.NewLine, result.Errors);
                    MessageBox.Show(errors, "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (viewModel.Data.Count > 0)
                {
                    viewModel.SelectedItem = viewModel.Data[0];
                }
            };
        }
    }
}