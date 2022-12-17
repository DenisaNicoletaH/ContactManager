using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for AddAddress.xaml
    /// </summary>
    public partial class AddAddress : Window
    {
       
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        int contact_id = 0;

        public AddAddress(int c_id)
        {
            InitializeComponent();
            contact_id = c_id;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string street = Street.Text;
                string city = City.Text;
                string state = State.Text;
                string postalCode = PostalCode.Text;
                string country = Country.Text;

                List<char> typeCodes = new List<char>();

                if (street.Equals("") || Street.Text.Equals("") || city.Equals("") || City.Text.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }
                if(state.Length > 2 || State.Text.Length > 2)
                {
                    MessageBox.Show("The state should be 2 characters only.");
                    return;
                }
                if (postalCode.Length > 6 || PostalCode.Text.Length > 6)
                {
                    MessageBox.Show("The postal code should be 6 characters only.");
                    return;
                }
                if(country.Length < 2  || Country.Text.Length < 2)
                {
                    MessageBox.Show("The country cannot be less than 2 characters.");
                    return;
                }
                char typeCode = Type.Text.ToUpper().ToCharArray()[0];

                using (SqlConnection con2 = new SqlConnection(connectionString))
                {
                    con2.Open();
                    SqlCommand cm = new SqlCommand("select Code from Type", con2);
                    SqlDataReader sdr = cm.ExecuteReader();
                    while (sdr.Read())
                    {
                        typeCodes.Add(sdr["Code"].ToString().ToCharArray()[0]);
                    }
                }

                bool isExist = false;
                foreach (char i in typeCodes)
                {
                    if (typeCode.Equals(i))
                    {
                        isExist = true;
                    }
                }
                if (!isExist)
                {
                    MessageBox.Show("Type code is not valid");
                    return;
                }

                DateTime currentTime = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Address(Street,City,State,PostalCode,CreateDate,UpdateDate,Active,Type_Code,Contact_Id,Country) VALUES(@Street,@City,@State,@PostalCode,@CreateDate,@UpdateDate,@Active,@Type_Code,@Contact_Id,@Country)", con);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@State", state);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@CreateDate", currentTime);
                command.Parameters.AddWithValue("@UpdateDate", currentTime);
                command.Parameters.AddWithValue("@Active", true);
                command.Parameters.AddWithValue("@Type_Code", typeCode);
                command.Parameters.AddWithValue("@Contact_Id", contact_id);
                command.Parameters.AddWithValue("@Country", country);
                command.ExecuteNonQuery();
                this.Close();
            }
        }
    }
}
