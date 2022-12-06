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
        public Window2()
        {
            InitializeComponent();
            var contact = dB.GetContact(1);
            this.listViewDetails.Items.Add(new Contact { Id = contact.Id, FirstName = contact.FirstName, MiddleName = contact.MiddleName, LastName = contact.LastName });
        }
        private void DeleteContact(Object sender, RoutedEventArgs e)
        {
            dB.DeleteContact(1);
            Button button = sender as Button;
            listViewDetails.SelectedIndex = 0;
            this.listViewDetails.Items.RemoveAt(listViewDetails.SelectedIndex);
        }
    }
}
