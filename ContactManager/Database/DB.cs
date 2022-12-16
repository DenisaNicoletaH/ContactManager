using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public void UpdateContact(int contactId, string firstName, string lastName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Contact SET FirstName=@FirstName, LastName=@LastName WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", contactId);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.ExecuteNonQuery();
            }
        }
        public List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Address WHERE Active = 1", con);
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
                    addresses.Add(address);
                }
                sdr.Close();
            }


            return addresses;
        }

        public List<Address> GetAddressesForContact(int contactId)
        {
            List<Address> addresses = new List<Address>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Address WHERE Contact_Id=@Contact_Id", con);
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
                    addresses.Add(address);
                }
                sdr.Close();
            }
            return addresses;
        }

        public void UpdateAddress(int addressId, string street, string city, string state, string postalCode, string typeCode)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Address SET Street=@Street, City=@City, State=@State, PostalCode=@PostalCode, Type_Code=@TypeCode WHERE Id = @Id;", con);
                command.Parameters.AddWithValue("@Id", addressId);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@State", state);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@TypeCode", typeCode);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAddress(int addressId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("UPDATE Address SET Active = 0 WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", addressId);
                command.ExecuteNonQuery();
            }
        }

        public List<Phone> getPhonesForContact(int contact_id)
        {
           List <Phone> phonesA = new List<Phone>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Phone WHERE Contact_Id=@Contact_Id", con);
                command.Parameters.AddWithValue("@Contact_Id", contact_id);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Phone phone = new Phone();
                    phone.Id = (int)sdr["Id"];
                    phone.PhoneNumber = sdr["Phone"].ToString();
                    phone.TypeCode = sdr["Type_Code"].ToString();
                    phonesA.Add(phone);


                }
                sdr.Close();
            }
            return phonesA;
            
        }
        
        public List<Email> getEmailsForContact(int contact_id)
        {
            List<Email> emails = new List<Email>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Email WHERE Contact_Id=@Contact_Id", con);
                command.Parameters.AddWithValue("@Contact_Id", contact_id);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Email email = new Email();
                    email.Id = (int)sdr["Id"];
                    email.EmailString = sdr["EmailAddress"].ToString();
                    emails.Add(email);
                }
                sdr.Close();
            }
            return emails;
        }

       





        //List of Contacts goes to the center of the dockpanel


        //GetContacts-Show a few details (but show all contacts)
        //GetContact-Show more details for an ID
        //GetAddress-Shows address for an address ID
        //GetAddressesForContact-Get all adresses for a contact ID(a bit of details)
    }
}
