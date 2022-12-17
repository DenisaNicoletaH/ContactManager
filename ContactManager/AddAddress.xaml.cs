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
                string phoneNumber = tb1.Text;
                List<char> typeCodes = new List<char>();

                if (phoneNumber.Equals("") || tb2.Text.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }
                char typeCode = tb2.Text.ToUpper().ToCharArray()[0];

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
                SqlCommand command = new SqlCommand("INSERT INTO Phone(Phone,Type_Code,CreateDate,UpdateDate,Active,Contact_Id) VALUES(@phone,@typeCode,@createDate,@updateDate,@active,@contactId)", con);
                command.Parameters.AddWithValue("@phone", phoneNumber);
                command.Parameters.AddWithValue("@typeCode", typeCode);
                command.Parameters.AddWithValue("@active", true);
                command.Parameters.AddWithValue("@createDate", currentTime);
                command.Parameters.AddWithValue("@updateDate", currentTime);
                command.Parameters.AddWithValue("contactId", contact_id);
                command.ExecuteNonQuery();
                this.Close();
            }
        }
    }
}
