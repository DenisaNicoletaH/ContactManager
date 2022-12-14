using ContactManager.Database.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace ContactManager.Database
{
    // Button to export the whole csv file: View (better than storedProceedure)
    // Last update date trigger
    // CSV file with "" to not confuse the commas
    internal class DB
    {
        //use sql command?
        //get Constructor
        // s=sql SQLCOMMAND--?

        //test push
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public void GetCSVFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files | *.txt";
            saveFileDialog.InitialDirectory = @"C:\";
            if (saveFileDialog.ShowDialog() == true) {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from Contact Where Active=1", con);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    Stream fileStream = saveFileDialog.OpenFile();
                    StreamWriter sw = new StreamWriter(fileStream);
                    int contactId;
                    while (sdr.Read())
                    {
                        contactId = Int32.Parse(sdr["Id"].ToString());
                        string contactCreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                        string contactUpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                        sw.WriteLine(sdr["Id"]+", " + sdr["FirstName"]+", " + sdr["MiddleName"]+", " + sdr["LastName"]+", " + contactCreatedDate + ", " + contactUpdatedDate);
                        sw.WriteLine();
                        using (SqlConnection con2 = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Address Where Active=1 and Contact_Id=@ContactId", con);
                            cmd2.Parameters.AddWithValue("@ContactId", contactId);
                            SqlDataReader sdr2 = cmd2.ExecuteReader();
                            while (sdr2.Read())
                            {
                                string addressCreatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["CreateDate"]);
                                string addressUpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["UpdateDate"]);
                                sw.WriteLine("\t" + sdr2["Id"] + ", " + sdr2["Contact_Id"] + ", " + sdr2["Street"] + ", " + sdr2["City"] + ", " + sdr2["State"] + ", " + sdr2["Country"] + ", " + sdr2["PostalCode"] + ", " + addressCreatedDate + ", " + addressUpdatedDate + sdr2["Type_Code"]);
                            }
                        }
                        sw.WriteLine();
                        using (SqlConnection con2 = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Phone Where Active=1 and Contact_Id=@ContactId", con);
                            cmd2.Parameters.AddWithValue("@ContactId", contactId);
                            SqlDataReader sdr2 = cmd2.ExecuteReader();
                            while (sdr2.Read())
                            {
                                string phoneCreatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["CreateDate"]);
                                string phoneUpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["UpdateDate"]);
                                sw.WriteLine("\t" + sdr2["Id"] + ", " + sdr2["Contact_Id"] +", "+ sdr2["Phone"] + ", " + sdr2["Type_Code"] + ", " + phoneCreatedDate + ", " + phoneUpdatedDate);
                            }
                        }
                        sw.WriteLine();
                        using (SqlConnection con2 = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Email Where Active=1 and Contact_Id=@ContactId", con);
                            cmd2.Parameters.AddWithValue("@ContactId", contactId);
                            SqlDataReader sdr2 = cmd2.ExecuteReader();
                            while (sdr2.Read())
                            {
                                string emailCreatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["CreateDate"]);
                                string emailUpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr2["UpdateDate"]);
                                sw.WriteLine("\t" + sdr2["Id"] + ", " + sdr2["Contact_Id"] + ", " + sdr2["EmailAddress"] + ", " + sdr2["Type_Code"] + ", " + emailCreatedDate + ", " + emailUpdatedDate);
                            }
                        }
                        sw.WriteLine();
                    }
                    sw.Close();
                    /*return CreateCSV(new SqlCommand("select * from Contact", con).ExecuteReader());*/
                }
            }
        }

        /*public void CreateCSV(IDataReader reader)
        {
            string file = @"c:\\";
            List<string> lines = new List<string>();
            string headerLine = "";
            if (reader.Read())
            {
                string[] columns = new string[reader.FieldCount];
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    columns[i] = reader.GetName(i);
                }
                headerLine = string.Join(",", columns);
                lines.Add(headerLine);
            }
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);
                lines.Add(string.Join(",", values));
            }
            System.IO.File.WriteAllLines(file, lines);
            return file;
        }*/

        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Contact WHERE Active = 1", con);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Contact contact = new Contact();
                    contact.Id = (int)sdr["Id"];
                    contact.FirstName = sdr["FirstName"].ToString();
                    contact.LastName = sdr["LastName"].ToString();
                    contact.MiddleName = sdr["MiddleName"].ToString();
                    contact.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    contact.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                    contacts.Add(contact);
                }
                sdr.Close();
            }

            
                return contacts;
        }

        //Get Contact
        public Contact GetContact(int contactId)
        {
            Contact contact = new Contact();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Contact WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    contact.Id = (int)sdr["Id"];
                    contact.FirstName = sdr["FirstName"].ToString();
                    contact.MiddleName = sdr["MiddleName"].ToString();
                    contact.LastName = sdr["LastName"].ToString();
                    contact.MiddleName = sdr["MiddleName"].ToString();
                    contact.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    contact.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                }
                sdr.Close();
            }
            return contact;
        }

        //Delete Contact 
        public void DeleteContact(int contactId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET Active = 0 WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateContact(int contactId, string firstName, string lastName, string middleName, int iId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime currentTime = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName, UpdateDate=@UpdatedDate, Image_Id=@ImageId WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.Parameters.AddWithValue("@ImageId", iId);
                command.ExecuteNonQuery();
            }
        }
        public Address GetAddress(int addressId)
        {
            Address address = new Address();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Address WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", addressId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    address.Id = (int)sdr["Id"];
                    address.Street = sdr["Street"].ToString();
                    address.City = sdr["City"].ToString();
                    address.State = sdr["State"].ToString();
                    address.Country = sdr["Country"].ToString();
                    address.PostalCode = sdr["PostalCode"].ToString();
                    address.TypeCode = sdr["Type_Code"].ToString();
                    address.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    address.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                }
                sdr.Close();
            }
            return address;
        }

        public List<Address> GetAddresses(int contactId)
        {
            List<Address> addresses = new List<Address>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Address WHERE Contact_Id=@Contact_Id AND Active=1", con);
                command.Parameters.AddWithValue("@Contact_Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Address address = new Address();
                    address.Id = (int)sdr["Id"];
                    address.Street = sdr["Street"].ToString();
                    address.City = sdr["City"].ToString();
                    address.State = sdr["State"].ToString();
                    address.Country = sdr["Country"].ToString();
                    address.PostalCode = sdr["PostalCode"].ToString();
                    address.TypeCode = sdr["Type_Code"].ToString();
                    address.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    address.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                    addresses.Add(address);
                }
                sdr.Close();
            }
            return addresses;
        }

        public void UpdateAddress(int contactId, int addressId, string street, string city, string state, string country, string postalCode, string typeCode)
        {
            DateTime currentTime = DateTime.Now;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Address SET Street=@Street, City=@City, State=@State, Country=@Country, PostalCode=@PostalCode, Type_Code=@TypeCode, UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", addressId);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@State", state);
                command.Parameters.AddWithValue("@Country", country);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@TypeCode", typeCode);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
        }

        public void AddAddress(int contactId, string street, string city, string state, string postalCode, DateTime currentTime, char typeCode, string country)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Address(Street,City,State,PostalCode,CreateDate,UpdateDate,Active,Type_Code,Contact_Id,Country) VALUES (@Street,@City,@State,@PostalCode,@CreateDate,@UpdateDate,@Active,@Type_Code,@Contact_Id,@Country);", con);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@State", state);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@CreateDate", currentTime);
                command.Parameters.AddWithValue("@UpdateDate", currentTime);
                command.Parameters.AddWithValue("@Active", true);
                command.Parameters.AddWithValue("@Type_Code", typeCode);
                command.Parameters.AddWithValue("@Contact_Id", contactId);
                command.Parameters.AddWithValue("@Country", country);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAddress(int contactId, int addressId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Address SET Active = 0 WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", addressId);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }

        public List<Phone> GetPhones(int contactId)
        {
           List <Phone> phonesA = new List<Phone>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Phone WHERE Contact_Id=@Contact_Id AND Active = 1", con);
                command.Parameters.AddWithValue("@Contact_Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Phone phone = new Phone();
                    phone.Id = (int)sdr["Id"];
                    phone.PhoneNumber = sdr["Phone"].ToString();
                    phone.TypeCode = sdr["Type_Code"].ToString();
                    phone.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    phone.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                    phonesA.Add(phone);
                }
                sdr.Close();
            }
            return phonesA;
            
        }
        public void UpdatePhone(int contactId, int addressId, string phoneNumber, string typeCode)
        {
            DateTime currentTime = DateTime.Now;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Phone SET Phone=@PhoneNumber, Type_Code=@TypeCode, UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", addressId);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@TypeCode", typeCode);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
        }

        public void AddPhone(int contactId, string phoneNumber, char typeCode, DateTime currentTime)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Phone(Phone,Type_Code,CreateDate,UpdateDate,Active,Contact_Id) VALUES(@phone,@typeCode,@createDate,@updateDate,@active,@contactId)", con);
                command.Parameters.AddWithValue("@phone", phoneNumber);
                command.Parameters.AddWithValue("@typeCode", typeCode);
                command.Parameters.AddWithValue("@active", true);
                command.Parameters.AddWithValue("@createDate", currentTime);
                command.Parameters.AddWithValue("@updateDate", currentTime);
                command.Parameters.AddWithValue("contactId", contactId);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }
        public void DeletePhone(int contactId, int phoneId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Phone SET Active = 0 WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", phoneId);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }

        public Phone GetPhone(int phoneId)
        {
            Phone phone = new Phone();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Phone WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", phoneId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    phone.Id = (int)sdr["Id"];
                    phone.PhoneNumber = sdr["Phone"].ToString();
                    phone.TypeCode = sdr["Type_Code"].ToString();
                    phone.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    phone.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                }
                sdr.Close();
            }
            return phone;
        }

        public List<Email> GetEmails(int contactId)
        {
            List<Email> emails = new List<Email>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Email WHERE Contact_Id=@Contact_Id AND Active=1", con);
                command.Parameters.AddWithValue("@Contact_Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Email email = new Email();
                    email.Id = (int)sdr["Id"];
                    email.EmailAddress = sdr["EmailAddress"].ToString();
                    email.TypeCode = sdr["Type_Code"].ToString();
                    email.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    email.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                    emails.Add(email);
                }
                sdr.Close();
            }
            return emails;
        }

        public Email GetEmail(int emailId)
        {
            Email email = new Email();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Email WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", emailId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    email.Id = (int)sdr["Id"];
                    email.EmailAddress = sdr["EmailAddress"].ToString();
                    email.TypeCode = sdr["Type_Code"].ToString();
                    email.CreatedDate = String.Format("{0:MM/dd/yyyy}", sdr["CreateDate"]);
                    email.UpdatedDate = String.Format("{0:MM/dd/yyyy}", sdr["UpdateDate"]);
                }
                sdr.Close();
            }
            return email;
        }

        public void UpdateEmail(int contactId, int emailId, string emailAddress, string typeCode)
        {
            DateTime currentTime = DateTime.Now;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Email SET EmailAddress=@EmailAddress, Type_Code=@TypeCode, UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", emailId);
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                command.Parameters.AddWithValue("@TypeCode", typeCode);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
        }

        public void AddEmail(int contactId, string email, DateTime currentTime, char typeCode)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Email(EmailAddress,CreateDate,UpdateDate,Active,Type_Code,Contact_Id) VALUES(@emailAddress,@createDate,@updateDate,@active,@typeCode,@contactId)", con);
                command.Parameters.AddWithValue("@emailAddress", email);
                command.Parameters.AddWithValue("@createDate", currentTime);
                command.Parameters.AddWithValue("@updateDate", currentTime);
                command.Parameters.AddWithValue("@active", true);
                command.Parameters.AddWithValue("@typeCode", typeCode);
                command.Parameters.AddWithValue("@contactId", contactId);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteEmail(int contactId, int emailId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Email SET Active = 0 WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", emailId);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DateTime time = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", time);
                command.ExecuteNonQuery();
            }
        }
        public ContactImage GetContactImage(int contactId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                ContactImage img = new ContactImage();
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Contact_Image Join Contact on Contact.Image_Id=Contact_Image.Id WHERE Contact.Id=@Id", con);
                command.Parameters.AddWithValue("@Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Image image = new Image();
                    MemoryStream ms = new MemoryStream((byte[])sdr["Image"]);
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = ms;
                    imageSource.EndInit();
                    image.Source = imageSource;
                    img.Id = Int32.Parse(sdr["Id"].ToString());
                    img.Img = image;
                    img.Description = sdr["Description"].ToString();
                    img.Img = image;
                }
                sdr.Close();
                return img;
            }
        }

        public void UpdateContactImage(int contactId, int imageId) 
        {
            DateTime currentTime = DateTime.Now;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET Image_Id=@ImageId WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@ImageId", imageId);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
                command.ExecuteNonQuery();
            }
        }

        public void AddContactImage(ContactImage cImage)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Contact_Image(Image,Description) VALUES(@Image,@Description)", con);
                command.Parameters.AddWithValue("@Image", cImage.ImageToByte);
                command.Parameters.AddWithValue("@Description", cImage.Description);
                command.ExecuteNonQuery();
            }
        }


        //List of Contacts goes to the center of the dockpanel


        //GetContacts-Show a few details (but show all contacts)
        //GetContact-Show more details for an ID
        //GetAddress-Shows address for an address ID
        //GetAddressesForContact-Get all adresses for a contact ID(a bit of details)
    }
}
