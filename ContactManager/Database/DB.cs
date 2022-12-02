using ContactManager.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContactManager.Database
{
    internal class DB
    {

        //test push
        string connectionString = "Server=localhost;Database=finalProjectDBF;Trusted_Connection=True";
          
        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Contact", con);
                SqlDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    Contact contact = new Contact();
                    contact.Id = (int)sdr["Id"];
                    contact.FirstName = sdr["FirstName"].ToString();
                    contact.LastName = sdr["LastName"].ToString();     
                    contacts.Add(contact);
                }

            }
                return contacts;
        }

        public Contact GetContact(int contactId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                Contact contact = new Contact();
                SqlCommand command = new SqlCommand("SELECT * FROM Contact WHERE Id=@Id", con);
                command.Parameters.AddWithValue("@Id", contactId);
            }
            Window2 win2 = new Window2();
            win2.Show();
            return null;
        }

        //List of Contacts goes to the center of the dockpanel
        //GetContacts-Show a few details (but show all contacts)
        //GetContact-Show more details for an ID
        //GetAddress-Shows an address for an address ID
        //GetAddressesForContact-Get all adresses for a contact ID(a bit of details)
    }
}
