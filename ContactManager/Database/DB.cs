using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

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
        string connectionString = "Server=localhost;Database=finalProjectDB;Trusted_Connection=True";
          
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

        public void UpdateContact(int contactId, string firstName, string lastName, string middleName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (middleName.Length > 1)
                {
                    MessageBox.Show("Middle name cannot be more than one character");
                    return;
                }

                DateTime currentTime = DateTime.Now;
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName, UpdateDate=@UpdatedDate WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@UpdatedDate", currentTime);
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







        //List of Contacts goes to the center of the dockpanel


        //GetContacts-Show a few details (but show all contacts)
        //GetContact-Show more details for an ID
        //GetAddress-Shows address for an address ID
        //GetAddressesForContact-Get all adresses for a contact ID(a bit of details)
    }
}
