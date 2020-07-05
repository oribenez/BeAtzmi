using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes
{
    [Table("Cities")]
    class City
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
