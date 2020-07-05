using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes
{
    [Table("Services")]
    class Service
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string infoFileName { get; set; }
        public AgeCode ageCode { get; set; }
        public GenderCode genderCode { get; set; }

        public string description { get; set; }

        public bool arab { get; set; }
        public bool religion { get; set; }
        public bool olim { get; set; }
        public bool singleParent { get; set; }
        public bool workImmigrations { get; set; }
        public bool etyopee { get; set; }
        public bool disabled { get; set; }
        public bool exPrisionerNAddicted { get; set; }
        public bool workersRightNleagalAssistence { get; set; }
        public bool baseStudy { get; set; }
        public bool proStudy { get; set; }
        public bool milgot { get; set; }
        public bool buissnesInitiatives { get; set; }
        public bool houseBudgetManage { get; set; }
        public bool GOVemploy { get; set; }
        public bool ORGemploy { get; set; }
        public bool israelEmployPlan { get; set; }
        public bool isDefault { get; set; }

        public override string ToString()
        {
            return ID.ToString() + ": " + name;
        }


        public enum GenderCode : int
        {
            MaleOnly = 0,
            FemaleOnly = 1,
            Both = 2
        }

        public enum AgeCode : int
        {
            EightteenTillThirtyFive = 0,
            FourtyFivePlus = 1,
            All = 2
        }
    }
}
