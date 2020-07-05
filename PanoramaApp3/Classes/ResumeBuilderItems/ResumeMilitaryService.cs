using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    class ResumeMilitaryService : IResumeBuilderItem
    {
        //Fields Definition
        private string _name = "שירות צבאי";
        private string _value;
        private ResumeBuilderItemKind _kind = ResumeBuilderItemKind.MilitaryService;
        private int _startYear, _endYear;
        private string _jobDetail, _place;

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
        public string Place
        {
            get
            {
                return _place;
            }

            set
            {
                _place = value;
            }
        }

        //Constructor
        public ResumeMilitaryService(string jobName, string jobDeatilText, int beginYear, int endYear, string where)
        {
            _value = jobName;
            _startYear = beginYear;
            _endYear = endYear;
            _jobDetail = jobDeatilText;
            _place = where;
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
            return _name + ": " + getYearPrintView() + ">" + _value;
        }

    }
}
