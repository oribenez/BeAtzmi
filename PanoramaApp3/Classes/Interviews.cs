using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes
{
    public sealed class Interviews
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public DateTime Datetime { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }
}
