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

        public Address GetAddressForContact(int contactId)
        {
            Address address = new Address();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Address WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", contactId);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    address.Id = (int)sdr["Id"];
                    address.Street = sdr["Street"].ToString();
                    address.City = sdr["City"].ToString();
                    address.PostalCode = sdr["PostalCode"].ToString();
                }
                sdr.Close();
            }
            return address;
        }


     
        //List of Contacts goes to the center of the dockpanel


        //GetContacts-Show a few details (but show all contacts)
        //GetContact-Show more details for an ID
        //GetAddress-Shows address for an address ID
        //GetAddressesForContact-Get all adresses for a contact ID(a bit of details)
    }
}
