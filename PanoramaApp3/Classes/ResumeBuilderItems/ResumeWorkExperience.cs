using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    class ResumeWorkExperience : IResumeBuilderItem
    {
        //Fields Definition
        private string _name = "נסיון תעסוקתי";
        private string _value;
        private ResumeBuilderItemKind _kind = ResumeBuilderItemKind.WorkExperience;
        private int _startYear, _endYear;
        private string _jobDetail;
        private string _role;

        //Property Definition
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
        public ResumeBuilderItemKind Kind
        {
            get
            {
                return _kind;
            }
        }
        public int StartYear
        {
            get
            {
                return _startYear;
            }

            set
            {
                _startYear = value;
            }
        }
        public int EndYear
        {
            get
            {
                return _endYear;
            }

            set
            {
                _endYear = value;
            }
        }
        public string JobDetail
        {
            get
            {
                return _jobDetail;
            }

            set
            {
                _jobDetail = value;
            }
        }
        public string Role
        {
            get
            {
                return _role;
            }

            set
            {
                _role = value;
            }
        }

        //Constructor
        public ResumeWorkExperience(string roleName, string companyName, string jobDeatilText, int beginYear, int endYear)
        {
            _role = roleName;
            _startYear = beginYear;
            _endYear = endYear;
            _jobDetail = jobDeatilText;
            _value = companyName;
        }

        private string getYearPrintView()
        {
            string ToReturn = _startYear.ToString() + " - ";
            if(_endYear == 0)
            {
                return ToReturn + "היום";
            }
            return ToReturn + _endYear.ToString();
        }

        public override string ToString()
        {
            return _name + ": " + getYearPrintView() + ">" + _role + ", " + _value;
        }

    }
}
