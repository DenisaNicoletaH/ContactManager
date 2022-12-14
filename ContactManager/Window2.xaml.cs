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
using System.Security.Policy;

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
        private string middleName;
        private int iId = 0;

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

        public string MiddleNameContact
        {
            get { return middleName; }
            set { middleName = value; OnPropertyChanged("MiddleNameContact"); }
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
            MiddleName.Text = contact.MiddleName;
            MiddleNameContact = contact.MiddleName;

            var image = dB.GetContactImage(id);
            pfp.Source = image.Img.Source;
            iId = image.Id;

            var addresses = dB.GetAddresses(id);
            foreach (var address in addresses)
            {
                this.addressDetails.Items.Add(new Address { Id = address.Id, Street = address.Street, City = address.City, State = address.State, Country = address.Country, CreatedDate=address.CreatedDate, UpdatedDate=address.UpdatedDate, PostalCode=address.PostalCode, TypeCode=address.TypeCode });
            }

            var emails = dB.GetEmails(id);
            foreach(var email in emails)
            {
                this.emailDetails.Items.Add(new Email { Id = email.Id, EmailAddress = email.EmailAddress, TypeCode = email.TypeCode, CreatedDate = email.CreatedDate, UpdatedDate = email.UpdatedDate });
            }
            var phones = dB.GetPhones(id);
            foreach (var phone in phones)
            {
                this.phoneDetails.Items.Add(new Phone { Id = phone.Id, PhoneNumber = phone.PhoneNumber, TypeCode=phone.TypeCode, CreatedDate=phone.CreatedDate, UpdatedDate=phone.UpdatedDate });
            }
        }
    
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            FirstName.IsReadOnly = false;
            LastName.IsReadOnly = false;
            MiddleName.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MiddleNameContact.Length > 1)
            {
                MessageBox.Show("Middle name cannot be more than one character");
                return;
            }
            dB.UpdateContact(contactId, FirstNameContact, LastNameContact, MiddleNameContact, iId);
            MainWindow contactsScreen = new MainWindow();
            contactsScreen.Show();
            contactsScreen.Focus();
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteContact(contactId);
            MainWindow contactsScreen = new MainWindow();
            contactsScreen.Show();
            contactsScreen.Focus();
            this.Close();
        }

        private void addressDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var address = addressDetails.SelectedItem as Address;
            var addressId = address.Id;
            AddressDetails detailsWindow = new AddressDetails(addressId,contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }

        private void phoneDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var phone = phoneDetails.SelectedItem as Phone;
            var phoneId = phone.Id;
            PhoneDetails detailsWindow = new PhoneDetails(contactId, phoneId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }
        private void emailDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var email = emailDetails.SelectedItem as Email;
            var emailId = email.Id;
            EmailDetails detailsWindow = new EmailDetails(contactId, emailId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }

        private void AddPhone_Click(object sender, RoutedEventArgs e)
        {
            AddPhone detailsWindow = new AddPhone(contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }

        private void AddAddress_Click(object sender, RoutedEventArgs e)
        {
            AddAddress addAddressWindow = new AddAddress(contactId);
            addAddressWindow.Show();
            addAddressWindow.Focus();
            this.Close();
        }

        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            AddEmail addEmailWindow = new AddEmail(contactId);
            addEmailWindow.Show();
            addEmailWindow.Focus();
            this.Close();
        }

        private void pfp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageDetails detailsWindow = new ImageDetails(contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }
    }
}
