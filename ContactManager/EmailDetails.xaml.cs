using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour EmailDetails.xaml
    /// </summary>
    public partial class EmailDetails : Window
    {
        DB dB = new DB();

        string emailAddress;
        string typeCode;
        int emailId;
        int contactId;
        public string EmailAddressEmail
        {
            get { return emailAddress; }
            set { emailAddress = value; OnPropertyChanged("EmailAddressPhone"); }
        }
        public string TypeCodeEmail
        {
            get { return typeCode; }
            set { typeCode = value; OnPropertyChanged("TypeCodePhone"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string properyChange)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(properyChange));
            }
        }
        public EmailDetails(int cId, int eId)
        {
            InitializeComponent();
            this.DataContext = this;
            var email = dB.GetEmail(eId);
            eAddress.Text = email.EmailAddress;
            EmailAddressEmail = email.EmailAddress;
            tCode.Text = email.TypeCode;
            TypeCodeEmail = email.TypeCode;
            emailId = eId;
            contactId = cId;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            eAddress.IsReadOnly = false;
            tCode.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";

            if (EmailAddressEmail.Equals("") || eAddress.Text.Equals(""))
            {
                MessageBox.Show("One or more of the fields above is empty");
                return;
            }

            Regex rx = new Regex(@".+\@.+\..+");
            bool matchedString = rx.IsMatch(EmailAddressEmail);

            if (!matchedString)
            {
                MessageBox.Show("The email should include the at sign \"@\" and the period as in \".com\"");
                return;
            }

            char typeCode = tCode.Text.ToUpper().ToCharArray()[0];
            List<char> typeCodes = new List<char>();
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
            dB.UpdateEmail(contactId, emailId, EmailAddressEmail, TypeCodeEmail);
            Window2 contactDetails = new Window2(contactId);
            contactDetails.Show();
            contactDetails.Focus();
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteEmail(contactId, emailId);
            Window2 contactDetails = new Window2(contactId);
            contactDetails.Show();
            contactDetails.Focus();
            this.Close();
        }
    }
}
