using ContactManager.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ContactManager
{
    /// <summary>
    /// Logique d'interaction pour AddPhone.xaml
    /// </summary>
    public partial class AddPhone : Window
    {
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        int contact_id = 0;
        DB dB = new DB();

        public AddPhone(int c_id)
        {
            InitializeComponent();
            contact_id = c_id;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string phoneNumber = phoneBox.Text;
                List<char> typeCodes = new List<char>();

                if (phoneNumber.Equals("") || tcBox.Text.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }
                char typeCode = tcBox.Text.ToUpper().ToCharArray()[0];

                Regex rx = new Regex(@"[a-z]+");
                bool matchedString = rx.IsMatch(phoneNumber);

                if (phoneNumber.Length != 10)
                {
                    MessageBox.Show("Phone number should be 10 digits");
                    return;
                }

                if (matchedString)
                {
                    MessageBox.Show("Phone number should only contain numbers");
                    return;
                }

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
                dB.AddPhone(contact_id, phoneNumber, typeCode, currentTime);
                this.Close();
            }
        }
    }
}
