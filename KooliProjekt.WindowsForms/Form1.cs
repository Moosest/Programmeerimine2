using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly IClientsApiClient _clientsApiClient;

        public Form1(IClientsApiClient clientsApiClient)
        {
            _clientsApiClient = clientsApiClient;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private static string BuildErrorMessage(OperationResult response)
        {
            var messages = new List<string>();

            if (response.Errors != null)
            {
                messages.AddRange(response.Errors.Where(e => !string.IsNullOrWhiteSpace(e)));
            }

            if (response.PropertyErrors != null)
            {
                messages.AddRange(response.PropertyErrors
                    .Where(e => !string.IsNullOrWhiteSpace(e.Value))
                    .Select(e => $"{e.Key}: {e.Value}"));
            }

            return messages.Count > 0
                ? string.Join(Environment.NewLine, messages)
                : "Operation failed.";
        }

        private async Task LoadData()
        {
            var response = await _clientsApiClient.List(1, 10);

            if (response?.HasErrors == true)
            {
                MessageBox.Show(BuildErrorMessage(response));
                return;
            }

            dataGridView1.DataSource = response?.Value?.Results;
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBoxDiscount.Text, out var discount))
            {
                MessageBox.Show("Discount peab olema number.");
                return;
            }

            var client = new Client
            {
                Name = textBoxName.Text,
                Email = textBoxEmail.Text,
                Phone = textBoxPhone.Text,
                Address = textBoxAddress.Text,
                Discount = discount
            };

            var response = await _clientsApiClient.Save(client);

            if (response?.HasErrors == true)
            {
                MessageBox.Show(BuildErrorMessage(response));
                return;
            }

            textBoxName.Clear();
            textBoxEmail.Clear();
            textBoxPhone.Clear();
            textBoxAddress.Clear();
            textBoxDiscount.Clear();

            await LoadData();
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is not Client client)
            {
                MessageBox.Show("Vali kustutatav rida.");
                return;
            }

            var response = await _clientsApiClient.Delete(client.Id);

            if (response?.HasErrors == true)
            {
                MessageBox.Show(BuildErrorMessage(response));
                return;
            }

            await LoadData();
        }
    }
}
