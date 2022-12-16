using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        public AddressDetails(int id)
        {
            InitializeComponent();
            this.DataContext = this;
            var addresses = dB.GetAddressesForContact(id);
            foreach (var address in addresses)
            {
                streetBox.Text = address.Street;
                StreetAddress = address.Street;
                cityBox.Text = address.City;
                CityAddress = address.City;
                stateBox.Text = address.State;
                StateAddress = address.State;
                CountryAddress = address.Country;
                pcBox.Text = address.PostalCode;
                PostalCodeAddress = address.PostalCode;
                tcBox.Text = address.TypeCode;
                TypeCodeAddress = address.TypeCode;
                addressId = id;
            }
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            streetBox.IsReadOnly = false;
            cityBox.IsReadOnly = false;
            stateBox.IsReadOnly = false;
            pcBox.IsReadOnly = false;
            tcBox.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dB.UpdateAddress(addressId, StreetAddress, CityAddress, StateAddress, PostalCodeAddress, TypeCodeAddress);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteAddress(addressId);
            this.Close();
        }
    }
}
