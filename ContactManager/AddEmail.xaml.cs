using ContactManager.Database;
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
    /// Interaction logic for AddEmail.xaml
    /// </summary>
    public partial class AddEmail : Window
    {
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        int contactId = 0;
        DB dB = new DB();

        public AddEmail(int c_id)
        {
            InitializeComponent();
            contactId = c_id;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string email = emailBox.Text;
                List<char> typeCodes = new List<char>();

                if (email.Equals("") || emailBox.Text.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }

                Regex rx = new Regex(@".+\@.+\..+");
                bool matchedString = rx.IsMatch(email);

                if (!matchedString)
                {
                    MessageBox.Show("The email isn't valid, check if it includes the at sign \"@\" and the period as in \".com\"");
                    return;
                }

                char typeCode = tcBox.Text.ToUpper().ToCharArray()[0];

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
                dB.AddEmail(contactId, email,currentTime,typeCode);
                this.Close();

            }
        }
    }
}
