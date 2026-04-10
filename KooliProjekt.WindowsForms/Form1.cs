using System.Net.Http.Json;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var url = "http://localhost:5086/api/Clients";
            url += "?page=1&pageSize=10";

            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<OperationResult<PagedResult<Client>>>(url);
            dataGridView1.DataSource = response?.Value?.Results;
        }
    }
}
