using ContactManager.Database;
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
        SqlConnection con = new SqlConnection("Server=localhost;Database=finalProjectDBF;Trusted_Connection=True");
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader;
        public MainWindow()
        {
            //this shows the window
            InitializeComponent();

            DB dB = new DB();
            var contacts = dB.GetContacts();
            foreach (var contact in contacts)
            {
                listViewList.Items.Add($"{contact.FirstName} { contact.LastName}");
            }
        }


    }
}
/* con.Open();
           command.CommandText = "SELECT * FROM Contact";
           command.Connection = con;
           dataReader = command.ExecuteReader();
           listViewList.Items.Clear();*/
/*while (dataReader.Read())
{  
    listViewList.Items.Add(dataReader.GetInt32(0).ToString());
}
dataReader.Close();*/


/* DB dB = new DB();
           var contacts = dB.GetContacts();
           foreach (var contact in contacts)
           {
               //btn1.Content = ($"{contact.FirstName} {contact.LastName}");

           }*/