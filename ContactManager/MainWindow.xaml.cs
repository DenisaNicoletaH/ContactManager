using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        public MainWindow()
        {
            //this shows the window
            InitializeComponent();

            DB dB = new DB();
            var contacts = dB.GetContacts();
            foreach (var contact in contacts)
            {
                this.listView.Items.Add(new Contact {Id=contact.Id, FirstName = contact.FirstName, LastName=contact.LastName});
            }
        }

        private void listView_MouseDoubleClick(object sender, SelectionChangedEventArgs e)
        {
            Window2 detailsWindow = new Window2();
            detailsWindow.Show();
        }
    }
}