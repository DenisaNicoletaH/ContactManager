using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ContactManager.Database.Entities
{
    internal class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }  
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

      
        //dont need to write the address cause theres another class for that
    }
}
