using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes
{
    class Lishkot
    {
        public string name { get; set; }
        public string link { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string location { get; set; }
        public double distance { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
