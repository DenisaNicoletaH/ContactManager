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

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        DB dB = new DB();
        int idOfContactToBeDeleted = 0;
        public Window2(int id)
        {

            //thereessss ann issueeeeeeeee hehehe for fixing later ;)
            InitializeComponent();
            var contact = dB.GetContact(id);
            FirstName.Text = contact.FirstName;
            LastName.Text = contact.LastName;
            //this.listViewDetails.Items.Add(new Contact { Id = contact.Id, FirstName = contact.FirstName, MiddleName = contact.MiddleName, LastName = contact.LastName});
        }
        private void DeleteContact(Object sender, RoutedEventArgs e)
        {
            dB.DeleteContact(idOfContactToBeDeleted);
            /*Button button = sender as Button;
             listViewDetails.SelectedIndex = listViewDetails.SelectedItem as Contact;
             this.listViewDetails.Items.RemoveAt(listViewDetails.SelectedIndex);*/

        }

        private void listViewDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*var contact = listViewDetails.SelectedItem as Contact;
            var contactId = contact.Id;
            idOfContactToBeDeleted = contactId;*/
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
            if()
        }
    }
}
