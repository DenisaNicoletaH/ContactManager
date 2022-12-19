using ContactManager.Database.Entities;
using ContactManager.Database;
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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ContactManager
{
    /// <summary>
    /// Logique d'interaction pour PhoneDetails.xaml
    /// </summary>
    public partial class PhoneDetails : Window
    {
        DB dB=new DB();

        string phoneNumber;
        string typeCode;
        int phoneId;
        int contactId;
        public string PhoneNumberPhone
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged("PhoneNumberPhone"); }
        }
        public string TypeCodePhone
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
        public PhoneDetails(int cId, int pId)
        {
            InitializeComponent();
            this.DataContext = this;
            var phone = dB.GetPhone(pId);
            pNumber.Text = phone.PhoneNumber;
            PhoneNumberPhone = phone.PhoneNumber;
            tCode.Text = phone.TypeCode;
            TypeCodePhone = phone.TypeCode;
            phoneId = pId;
            contactId = cId;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            pNumber.IsReadOnly = false;
            tCode.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PhoneNumberPhone.Equals("") || pNumber.Text.Equals("") || TypeCodePhone.Equals("") || tCode.Equals(""))
            {
                MessageBox.Show("One or more of the fields above is empty");
                return;
            }
            if(PhoneNumberPhone.Length != 10 || pNumber.Text.Length != 10)
            {
                MessageBox.Show("Phone number should be 10 digits");
                return;
            }

            Regex rx = new Regex(@"[a-z]+");
            bool matchedString = rx.IsMatch(PhoneNumberPhone);
            bool matchedBox = rx.IsMatch(pNumber.Text);

            if (matchedBox && matchedString)
            {
                MessageBox.Show("Phone number should only contain numbers");
                return;
            }
            string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";

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
            char typeCode = tCode.Text.ToUpper().ToCharArray()[0];

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
            dB.UpdatePhone(contactId, phoneId, PhoneNumberPhone, TypeCodePhone);
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeletePhone(contactId, phoneId);
            this.Close();
        }
    }
}
