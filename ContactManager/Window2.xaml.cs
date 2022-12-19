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
        int contactId = 0;
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
            contactId = id;
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
                this.emailDetails.Items.Add(new Email { Id = email.Id, EmailAddress = email.EmailAddress });
            }

            var phones = dB.getPhonesForContact(id);
            foreach (var phone in phones)
            {
                this.phoneDetails.Items.Add(new Phone { Id = phone.Id, PhoneNumber = phone.PhoneNumber, TypeCode=phone.TypeCode });
            }
        }
    
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            FirstName.IsReadOnly = false;
            LastName.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dB.UpdateContact(contactId, FirstNameContact, LastNameContact);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteContact(contactId);
            this.Close();
        }

        private void addressDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var address = addressDetails.SelectedItem as Address;
            var addressId = address.Id;
            AddressDetails detailsWindow = new AddressDetails(addressId,contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            //this.Close();
        }

        private void phoneDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var phone = phoneDetails.SelectedItem as Phone;
            var phoneId = phone.Id;
            PhoneDetails detailsWindow = new PhoneDetails(contactId, phoneId);
            detailsWindow.Show();
            detailsWindow.Focus();
        }
        private void emailDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var email = emailDetails.SelectedItem as Email;
            var emailId = email.Id;
            EmailDetails detailsWindow = new EmailDetails(contactId, emailId);
            detailsWindow.Show();
            detailsWindow.Focus();
            //this.Close();
        }

        private void AddPhone_Click(object sender, RoutedEventArgs e)
        {
            AddPhone detailsWindow = new AddPhone(contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            //this.Close();
        }

        private void AddAddress_Click(object sender, RoutedEventArgs e)
        {
            AddAddress addAddressWindow = new AddAddress(contactId);
            addAddressWindow.Show();
            addAddressWindow.Focus();

        }

        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            AddEmail addEmailWindow = new AddEmail(contactId);
            addEmailWindow.Show();
            addEmailWindow.Focus();
        }
    }
}
