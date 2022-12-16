using ContactManager.Database.Entities;
using ContactManager.Database;
using System;
using System.Collections.Generic;
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
using System.Runtime.ConstrainedExecution;
using System.ComponentModel;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window, INotifyPropertyChanged
    {
        DB dB = new DB();
        int idOfContactToBeDeleted = 0;
        private string firstName;
        private string lastName;

        public string FirstNameContact
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged("FirstNameContact"); }
        }

        public string LastNameContact
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged("LastNameContact"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string properyChange)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(properyChange));
            }
        }

        public Window2(int id)
        {

            InitializeComponent();
            this.DataContext = this;
            var contact = dB.GetContact(id);
            idOfContactToBeDeleted = id;
            FirstName.Text = contact.FirstName;
            FirstNameContact = contact.FirstName;
            LastName.Text = contact.LastName;
            LastNameContact = contact.LastName;
            var addresses = dB.GetAddressesForContact(id);
            foreach (var address in addresses)
            {
                this.addressDetails.Items.Add(new Address { Id = address.Id, Street = address.Street, City = address.City, State = address.State, Country = address.Country /*PostalCode=address.PostalCode*/ });
            }

            var emails = dB.getEmailsForContact(id);
            foreach(var email in emails)
            {
                this.emaildetails.Items.Add(new Email { Id = email.Id, EmailString = email.EmailString });
            }

            var phones = dB.getPhonesForContact(id);
            foreach (var phone in phones)
            {
                this.phonedetails.Items.Add(new Phone { Id = phone.Id, PhoneNumber = phone.PhoneNumber, TypeCode=phone.TypeCode });
            }
        }
    
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            FirstName.IsReadOnly = false;
            LastName.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dB.UpdateContact(idOfContactToBeDeleted, FirstNameContact, LastNameContact);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteContact(idOfContactToBeDeleted);
            this.Close();
        }

        private void addressDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var address = addressDetails.SelectedItem as Address;
            var addressId = address.Id;
            AddressDetails detailsWindow = new AddressDetails(addressId);
            detailsWindow.Show();
            detailsWindow.Focus();
            //this.Close();
        }

        private void phonedetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
