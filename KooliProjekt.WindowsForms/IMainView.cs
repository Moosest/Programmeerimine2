using System.ComponentModel;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        IList<Client> DataSource { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Client SelectedItem { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        int CurrentId { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string CurrentName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string CurrentEmail { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string CurrentPhone { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string CurrentAddress { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string CurrentDiscount { get; set; }

        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        bool ConfirmDelete();
    }
}