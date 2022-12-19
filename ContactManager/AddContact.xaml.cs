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
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

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
                string fName = tb1.Text;
                string lName = tb2.Text;
                string mName = tb3.Text;
                string active = trueCheckbox.IsChecked.ToString();
                DateTime currentTime = DateTime.Now;

                string iId = tb5.Text;
                int imageId=-1;
                ArrayList imageIds = new ArrayList();

                /*
                if (fName.Equals("") || lName.Equals("") || active.Equals("") ||
                    mName.Equals("") || iId.Equals(""))
                {
                    MessageBox.Show("One or more of the fields above is empty");
                    return;
                }
                */
               
                if (mName.Length > 1)
                {
                    MessageBox.Show("Middle name can only be one character");
                    return;
                }

                /*
               if (!active.ToLower().Equals("true") && !active.ToLower().Equals("false"))
                {
                    MessageBox.Show("Active field is not valid! Please enter true or false");
                    return;
                }
                */

                bool validId = Int32.TryParse(iId,out imageId);
                if (!validId || imageId<=0)
                {
                    MessageBox.Show("Image id is not a valid number");
                    return;
                }

                using (SqlConnection con2 = new SqlConnection(connectionString))
                {
                    con2.Open();
                    SqlCommand cm = new SqlCommand("select Id from Contact_Image", con2);
                    SqlDataReader sdr = cm.ExecuteReader();
                    while (sdr.Read())
                    {
                        imageIds.Add(Int32.Parse(sdr["Id"].ToString()));
                    }
                }

                bool isExist = false;
                foreach (int i in imageIds)
                {
                    if (imageId == i)
                    { 
                        isExist = true;
                    }
                }
                if (!isExist)
                {
                    MessageBox.Show("Image id is not valid");
                    return;
                }
                
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Contact(FirstName,LastName,MiddleName,Active,Image_Id,CreateDate,UpdateDate) VALUES(@firstName,@lastName,@middleName,@active,@imageId,@createDate,@updateDate)", con);
                command.Parameters.AddWithValue("@firstName",fName);
                command.Parameters.AddWithValue("@lastName", lName);
                command.Parameters.AddWithValue("@middleName", mName);
                command.Parameters.AddWithValue("@active", true);
                command.Parameters.AddWithValue("@imageId", iId);
                command.Parameters.AddWithValue("@createDate", currentTime);
                command.Parameters.AddWithValue("@updateDate", currentTime);
                command.ExecuteNonQuery();
                this.Close();
            }
        }
    }
}
