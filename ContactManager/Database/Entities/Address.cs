using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Database.Entities
{
    internal class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string TypeCode { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedDate { get; set; }

    }
}
