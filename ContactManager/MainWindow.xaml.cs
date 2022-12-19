using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
              DB dB = new DB();
        public MainWindow()
        {
            //this shows the window
            InitializeComponent();
           

      
            var contacts = dB.GetContacts();
            // var phoneNumbers = dB.getPhoneForContact();
            foreach (var contact in contacts)
            {
                this.listView.Items.Add(new Contact { Id = contact.Id, FirstName = contact.FirstName, LastName = contact.LastName, CreatedDate=contact.CreatedDate, UpdatedDate=contact.UpdatedDate, MiddleName = contact.MiddleName });

            }
           

        }

        //Double Click The Contact
        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var contact = listView.SelectedItem as Contact;
            var contactId = contact.Id;
            Window2 detailsWindow = new Window2(contactId);
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            AddContact detailsWindow = new AddContact();
            detailsWindow.Show();
            detailsWindow.Focus();
            this.Close();
        }

        private void AtoZ_Click(object sender, RoutedEventArgs e)
        {

            List<Contact> contacts = dB.GetContacts();
            if(AtoZ.IsPressed == true)
            {

                var sortedList = contacts.OrderBy(x => x.FirstName).ToList();
            }
           

        }

        private void ZtoA_Click(object sender, RoutedEventArgs e)
        {
            List<Contact> contacts = dB.GetContacts();

            if (ZtoA.IsPressed == true)
            {

                var sortedList = contacts.OrderBy(x => x.FirstName).Reverse().ToList();
            }
        }

        private void IdCheckbox(object sender, RoutedEventArgs e)
        {
            List<Contact> contacts = dB.GetContacts();
            if (IdOrder.IsChecked == true)
            {
                var sortedId=contacts.OrderBy(x => x.Id).ToList();
            }
            else
            {
                var sortedId=contacts.OrderBy(x => x.Id).Reverse().ToList();
            }


            {

            }
        }
    }

    
}