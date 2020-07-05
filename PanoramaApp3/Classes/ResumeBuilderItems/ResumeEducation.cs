using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    class ResumeEducation : IResumeBuilderItem
    {
        //Fields Definition
        private string _name = "השכלה";
        private string _value;
        private ResumeBuilderItemKind _kind = ResumeBuilderItemKind.Education;
        private int _startYear, _endYear;
        private string _detail;
        private string _certificate;
        private string _specialization;

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
        public string Detail
        {
            get
            {
                return _detail;
            }

            set
            {
                _detail = value;
            }
        }
        public string Certificate
        {
            get
            {
                return _certificate;
            }

            set
            {
                _certificate = value;
            }
        }
        public string Specialization
        {
            get
            {
                return _specialization;
            }

            set
            {
                _specialization = value;
            }
        }

        //Constructor
        public ResumeEducation(int startYear, int endYear, string institution, string certificate, string special, string details)
        {
            _startYear = startYear;
            _endYear = endYear;
            _value = institution;
            _certificate = certificate;
            _specialization = special;
            _detail = details;
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
