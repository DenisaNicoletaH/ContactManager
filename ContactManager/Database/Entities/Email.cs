using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Database.Entities
{
    internal class Email
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string TypeCode { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
    }
}
