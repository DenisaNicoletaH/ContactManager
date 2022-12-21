using ContactManager.Database;
using ContactManager.Database.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Logique d'interaction pour AddImage.xaml
    /// </summary>
    public partial class AddImage : Window
    {
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
        ContactImage image = new ContactImage();
        private string path;
        DB dB = new DB();
        public AddImage()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg";
            openFileDialog1.DefaultExt = ".jpeg";
            path = openFileDialog1.FileName;
            ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
            imagePreview.Source = imageSource;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string description = tb2.Text;
                if (tb2.Text.Equals("") || imagePreview.Source.ToString().Equals(""))
                {
                    MessageBox.Show("One or more of the above fields is empty");
                    return;
                }

                image.ImagePath = path;
                image.ImageToByte = File.ReadAllBytes(path);
                /*con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Contact_Image(Image,Description) VALUES(@Image,@Description)", con);
                command.Parameters.AddWithValue("@Image", image.ImageToByte);
                command.Parameters.AddWithValue("@Description", description);
                command.ExecuteNonQuery();*/

                image.Description = description;
                dB.AddContactImage(image);
                MainWindow contactsScreen = new MainWindow();
                contactsScreen.Show();
                contactsScreen.Focus();
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow contactsScreen = new MainWindow();
            contactsScreen.Show();
            contactsScreen.Focus();
            this.Close();
        }
    }
}
