using ContactManager.Database;
using ContactManager.Database.Entities;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
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
using static System.Net.Mime.MediaTypeNames;

namespace ContactManager
{
    /// <summary>
    /// Logique d'interaction pour ImageDetails.xaml
    /// </summary>
    public partial class ImageDetails : Window
    {
        DB dB = new DB();
        int contactId = 0;
        int imageId=0;
        private string image;
        private string description;
        private string path;
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        
        public string ImageContactImage
        {
            get { return image; }
            set { image = value; OnPropertyChanged("ImageContactImage"); }
        }

        public string DescriptionContactImage
        {
            get { return description; }
            set { description = value; OnPropertyChanged("DescriptionContactImage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string properyChange)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(properyChange));
            }
        }
        public ImageDetails(int cId)
        {
            InitializeComponent();
            this.DataContext = this;
            contactId = cId;
            var img = dB.GetContactImage(cId);
            iId.Text = img.Id.ToString();       
            ImageContactImage = img.Id.ToString();
            descr.Text = img.Description;
            DescriptionContactImage = img.Description;
            imagePreview.Source = img.Img.Source;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            iId.IsReadOnly = false;
            descr.IsReadOnly = false;
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            iId.Text = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg";
            openFileDialog1.DefaultExt = ".jpeg";
            path = openFileDialog1.FileName;
            ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
            imagePreview.Source = imageSource;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (iId.Equals("") && imagePreview.Source.ToString().Equals(""))
            {
                MessageBox.Show("Please enter an existing image id or browse for an image");
                return;
            }
            if (DescriptionContactImage.Equals(""))
            {
                MessageBox.Show("Description cannot be empty");
                return;
            }

            ArrayList imageIds = new ArrayList();
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

            if (imagePreview.Source.ToString().Equals(""))
            { 
                imageId = Int32.Parse(iId.Text);
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
            }
            
            ContactImage ci = new ContactImage();
            if (iId.Text.Equals(""))
            {
                ci.Description = DescriptionContactImage;
                ci.ImagePath = path;
                ci.ImageToByte = File.ReadAllBytes(path);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Contact_Image(Image,Description) VALUES(@Image,@Description)", con);
                    command.Parameters.AddWithValue("@Image", ci.ImageToByte);
                    command.Parameters.AddWithValue("@Description", ci.Description);
                    command.ExecuteNonQuery();
                    string query2 = "Select @@Identity as imageId from Contact_Image";
                    command.CommandText = query2;
                    command.CommandType = CommandType.Text;
                    imageId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            else 
            {
                imageId = Int32.Parse(iId.Text);
                dB.UpdateContactImage(contactId, imageId);
            }

            var contactUpdated = dB.GetContact(contactId);
            dB.UpdateContact(contactId, contactUpdated.FirstName, contactUpdated.LastName, contactUpdated.MiddleName, imageId);

            Window2 contactDetails = new Window2(contactId);
            contactDetails.Show();
            contactDetails.Focus();
            this.Close();
        }
    }
}
