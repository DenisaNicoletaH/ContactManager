using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Database.Entities
{
    internal class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

public Phone(int id, string phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }

        public Phone()
        {
        }
    }
}
