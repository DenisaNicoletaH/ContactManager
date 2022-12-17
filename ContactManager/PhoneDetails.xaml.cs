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
        public PhoneDetails(int id)
        {
            InitializeComponent();
            this.DataContext = this;
            var phone = dB.GetPhone(id);
            pNumber.Text = phone.PhoneNumber;
            PhoneNumberPhone = phone.PhoneNumber;
            tCode.Text = phone.TypeCode;
            TypeCodePhone = phone.TypeCode;
            phoneId = id;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            pNumber.IsReadOnly = false;
            tCode.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dB.UpdatePhone(phoneId, PhoneNumberPhone, TypeCodePhone);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            dB.DeletePhone(phoneId);
            this.Close();
        }
    }
}
