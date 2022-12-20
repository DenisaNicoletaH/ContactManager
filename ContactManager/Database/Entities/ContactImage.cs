using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Database.Entities
{
    internal class ContactImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public byte[] ImageToByte { get; set; }
        public string Description { get; set; }
    }
}
