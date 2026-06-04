using System.ComponentModel;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form, IMainView
    {
        private MainViewPresenter _mainViewPresenter;
        private Client _selectedItem;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Client> DataSource
        {
            get { return (IList<Client>)dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Client SelectedItem
        {
            get
            {
                return dataGridView1.CurrentRow?.DataBoundItem as Client;
            }
            set
            {
                _selectedItem = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentId
        {
            get { return int.TryParse(textBoxId.Text, out var id) ? id : 0; }
            set { textBoxId.Text = value.ToString(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentName
        {
            get { return textBoxName.Text; }
            set { textBoxName.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentEmail
        {
            get { return textBoxEmail.Text; }
            set { textBoxEmail.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentPhone
        {
            get { return textBoxPhone.Text; }
            set { textBoxPhone.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentAddress
        {
            get { return textBoxAddress.Text; }
            set { textBoxAddress.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentDiscount
        {
            get { return textBoxDiscount.Text; }
            set { textBoxDiscount.Text = value; }
        }

        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        public void SetPresenter(MainViewPresenter presenter)
        {
            _mainViewPresenter = presenter;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await _mainViewPresenter.LoadData();
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

        public void ShowError(string message, OperationResult result)
        {
            var errorMessage = message;
            var details = BuildErrorMessage(result);

            if (!string.IsNullOrWhiteSpace(details))
            {
                errorMessage += Environment.NewLine + details;
            }

            MessageBox.Show(errorMessage, "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Save();
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            await _mainViewPresenter.Delete();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            _mainViewPresenter.SetSelection(SelectedItem);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            _mainViewPresenter.AddNew();
        }
    }
}
