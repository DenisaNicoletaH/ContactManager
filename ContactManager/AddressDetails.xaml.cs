using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContactManager
{
    /// <summary>
    /// Logique d'interaction pour AddressDetails.xaml
    /// </summary>
    public partial class AddressDetails : Window, INotifyPropertyChanged
    {
        DB dB = new DB();
        private string street;
        private string city;
        private string state;
        private string country;
        private string postalCode;
        private string typeCode;
        int addressId = 0;
        int contactId = 0;
        public string StreetAddress
        {
            get { return street; }
            set { street = value; OnPropertyChanged("StreetAddress"); }
        }

        public string CityAddress
        {
            get { return city; }
            set { city = value; OnPropertyChanged("CityAddress"); }
        }


        public string StateAddress
        {
            get { return state; }
            set { state = value; OnPropertyChanged("StateAddress"); }
        }

        public string CountryAddress
        {
            get { return country; }
            set { country = value; OnPropertyChanged("CountryAddress"); }
        }
        
        public string PostalCodeAddress
        {
            get { return postalCode; }
            set { postalCode = value; OnPropertyChanged("PostalCodeAddress"); }
        }
        public string TypeCodeAddress
        {
            get { return typeCode; }
            set { typeCode = value; OnPropertyChanged("TypeCodeAddress"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string properyChange)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(properyChange));
            }
        }
        public AddressDetails(int aId, int cId)
        {
            InitializeComponent();
            this.DataContext = this;
            var address = dB.GetAddress(aId);
            streetBox.Text = address.Street;
            StreetAddress = address.Street;
            cityBox.Text = address.City;
            CityAddress = address.City;
            stateBox.Text = address.State;
            StateAddress = address.State;
            countryBox.Text = address.Country;
            CountryAddress = address.Country;
            pcBox.Text = address.PostalCode;
            PostalCodeAddress = address.PostalCode;
            tcBox.Text = address.TypeCode;
            TypeCodeAddress = address.TypeCode;
            addressId = aId;
            contactId = cId;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            streetBox.IsReadOnly = false;
            cityBox.IsReadOnly = false;
            stateBox.IsReadOnly = false;
            countryBox.IsReadOnly = false;
            pcBox.IsReadOnly = false;
            tcBox.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";

            if (StreetAddress.Equals("") || streetBox.Text.Equals("") ||
                CityAddress.Equals("") || cityBox.Text.Equals("") || 
                CountryAddress.Equals("") || countryBox.Text.Equals(""))
            {
                MessageBox.Show("One or more of the fields above is empty");
                return;
            }
            if (StateAddress.Length != 2 || stateBox.Text.Length != 2)
            {
                MessageBox.Show("The state should be 2 characters only.");
                return;
            }
            Regex rxState = new Regex(@"[0-9]");
            bool matchedStringState = rxState.IsMatch(StateAddress);
            bool matchedStringCity = rxState.IsMatch(CityAddress);
            bool matchedStringCountry = rxState.IsMatch(CountryAddress);

            if (matchedStringState)
            {
                MessageBox.Show("The state should only contain letters.");
                return;
            }
            if (matchedStringCity)
            {
                MessageBox.Show("The city should only contain letters.");
                return;
            }
            if (matchedStringCountry)
            {
                MessageBox.Show("The country should only contain letters.");
                return;
            }
            if (PostalCodeAddress.Length > 6 || pcBox.Text.Length > 6)
            {
                MessageBox.Show("The postal code should be 6 characters only and it should be in this format: \"LNLNLN\" like \"J4W1W6\".");
                return;
            }
            Regex rxPostalCode = new Regex(@"^(?:[a-zA-Z]\d[a-zA-Z][ -]?\d[a-zA-Z]\d)$");
            bool matchedStringPostalCode = rxPostalCode.IsMatch(PostalCodeAddress);

            if (!matchedStringPostalCode)
            {
                MessageBox.Show("The postal code should be in this format \"LNLNLN\" like \"J4W1W6\".");
                return;
            }
            if (CountryAddress.Length < 2 || countryBox.Text.Length < 2)
            {
                MessageBox.Show("The country cannot be less than 2 characters.");
                return;
            }
            char typeCode = tcBox.Text.ToUpper().ToCharArray()[0];

            List<char> typeCodes = new List<char>();
            using (SqlConnection con2 = new SqlConnection(connectionString))
            {
                con2.Open();
                SqlCommand cm = new SqlCommand("select Code from Type", con2);
                SqlDataReader sdr = cm.ExecuteReader();
                while (sdr.Read())
                {
                    typeCodes.Add(sdr["Code"].ToString().ToCharArray()[0]);
                }
            }

            bool isExist = false;
            foreach (char i in typeCodes)
            {
                if (typeCode.Equals(i))
                {
                    isExist = true;
                }
            }
            if (!isExist)
            {
                MessageBox.Show("Type code is not valid");
                return;
            }

            DateTime currentTime = DateTime.Now;
            dB.UpdateAddress(contactId, addressId, StreetAddress, CityAddress, StateAddress, CountryAddress, PostalCodeAddress, TypeCodeAddress);
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteAddress(contactId,addressId);
            this.Close();
        }
    }
}
