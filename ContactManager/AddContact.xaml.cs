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
using System.Data.SqlClient;

namespace ContactManager
{
    /// <summary>
    /// Logique d'interaction pour AddContact.xaml
    /// </summary>
    public partial class AddContact : Window
    {
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        public AddContact()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //incomplete
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Contact(FirstName,LastName,Active,CreatedDate,UpdatedDate,MiddleName,Image_Id) VALUES(@firstName,@lastName,@active,@createdDate,@updatedDate,@middleName,@imageId)", con);
                command.Parameters.AddWithValue("@firstName",tb1.Text);
                command.Parameters.AddWithValue("@lastName", tb2.Text);
                command.Parameters.AddWithValue("@active", tb3.Text);
                command.Parameters.AddWithValue("@createdDate", tb4.Text);
                command.Parameters.AddWithValue("@updatedDate", tb5.Text);
                command.Parameters.AddWithValue("@middleName", tb6.Text);
                command.Parameters.AddWithValue("@imageId", tb7.Text);
                command.ExecuteNonQuery();
            }
        }
    }
}
