using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        int contactId = 0;
        DB dB = new DB();

        public AddAddress(int c_id)
        {
            InitializeComponent();
            contactId = c_id;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) 
        {
            Window2 contactDetails = new Window2(contactId);
            contactDetails.Show();
            contactDetails.Focus();
            this.Close();
        }

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

                Regex rxState = new Regex(@"[0-9]");
                bool matchedStringState = rxState.IsMatch(state);
                bool matchedStringCity = rxState.IsMatch(city);
                bool matchedStringCountry = rxState.IsMatch(country);

                if (matchedStringState)
                {
                    MessageBox.Show("The state should only contain letters.");
                    return;
                }
                if (matchedStringCity)
                {
                    MessageBox.Show("The city should only contain letters.");
                    return;
                }
                if (matchedStringCountry)
                {
                    MessageBox.Show("The country should only contain letters.");
                    return;
                }

                if (street.Equals("") || Street.Text.Equals("") || city.Equals("") || City.Text.Equals("")||Type.Text.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }
                if(state.Length != 2 || State.Text.Length != 2)
                {
                    MessageBox.Show("The state should be 2 characters only.");
                    return;
                }
                if (postalCode.Length > 6 || PostalCode.Text.Length > 6)
                {
                    MessageBox.Show("The postal code should be 6 characters only and it should be in this format: \"LNLNLN\" like \"J4W1W6\".");
                    return;
                }
                Regex rx = new Regex(@"^(?:[a-zA-Z]\d[a-zA-Z][ -]?\d[a-zA-Z]\d)$");
                bool matchedString = rx.IsMatch(postalCode);

                if (!matchedString)
                {
                    MessageBox.Show("The postal code should be in this format \"LNLNLN\" like \"J4W1W6\".");
                    return;
                }

                if (country.Length < 2  || Country.Text.Length < 2)
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
                dB.AddAddress(contactId, street, city, state, postalCode, currentTime, typeCode, country);
                Window2 contactDetails = new Window2(contactId);
                contactDetails.Show();
                contactDetails.Focus();
                this.Close();
            }
        }
    }
}
