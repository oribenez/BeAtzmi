using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    class ResumeLanguage : IResumeBuilderItem
    {
        //Fields Definition
        private string _name = "שפה";
        private string _value;
        private ResumeBuilderItemKind _kind = ResumeBuilderItemKind.Languages;
        private LanguageKnowledgeLevel _level;

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
        public LanguageKnowledgeLevel Level
        {
            get
            {
                return _level;
            }

            set
            {
                _level = value;
            }
        }

        //Constructor
        public ResumeLanguage(string languageName, LanguageKnowledgeLevel knowledgeLevel)
        {
            _value = languageName;
            _level = knowledgeLevel;
        }

        public override string ToString()
        {
            string ToReturn = _name + ": " + _value + ", ברמה: ";

            switch (_level)
            {
                case LanguageKnowledgeLevel.BasicLevel:
                    ToReturn += "בסיסית";
                    break;
                case LanguageKnowledgeLevel.BirthLanguage:
                    ToReturn += "שפת אם";
                    break;
                case LanguageKnowledgeLevel.BirthLanguageLevel:
                    ToReturn += "כמו שפת אם";
                    break;
                case LanguageKnowledgeLevel.GoodLevel:
                    ToReturn += "טובה";
                    break;
                case LanguageKnowledgeLevel.VeryGoodLevel:
                    ToReturn += "טובה מאוד";
                    break;
            }

            return ToReturn;
        }
    }

    enum LanguageKnowledgeLevel : int
    {
        BirthLanguage = 4,
        BirthLanguageLevel = 3,
        VeryGoodLevel = 2,
        GoodLevel = 1,
        BasicLevel = 0
    }
}
