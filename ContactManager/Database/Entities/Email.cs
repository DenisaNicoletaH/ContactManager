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
        public string EmailString { get; set; }
        public string Type { get; set; }
       
    }
}
