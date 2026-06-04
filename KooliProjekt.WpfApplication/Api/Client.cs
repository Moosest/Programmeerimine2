namespace KooliProjekt.WpfApplication
{
    public class Client : NotifyPropertyChangedBase
    {
        private int _id;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _address = string.Empty;
        private decimal _discount;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyPropertyChanged();
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                NotifyPropertyChanged();
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
                NotifyPropertyChanged();
            }
        }
    }
}