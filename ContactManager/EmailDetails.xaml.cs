using ContactManager.Database;
using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour EmailDetails.xaml
    /// </summary>
    public partial class EmailDetails : Window
    {
        DB dB = new DB();

        string emailAddress;
        string typeCode;
        int emailId;
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
        public EmailDetails(int id)
        {
            InitializeComponent();
            this.DataContext = this;
            var email = dB.GetEmail(id);
            eAddress.Text = email.EmailAddress;
            EmailAddressEmail = email.EmailAddress;
            tCode.Text = email.TypeCode;
            TypeCodeEmail = email.TypeCode;
            emailId = id;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            eAddress.IsReadOnly = false;
            tCode.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dB.UpdateEmail(emailId, EmailAddressEmail, TypeCodeEmail);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeleteEmail(emailId);
            this.Close();
        }
    }
}
