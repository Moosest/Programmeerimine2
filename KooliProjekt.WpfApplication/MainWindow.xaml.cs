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
                await viewModel.LoadDataAsync();

                if (viewModel.Data.Count > 0)
                {
                    viewModel.SelectedItem = viewModel.Data[0];
                }
            };
        }
    }
}