using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAtzmi.Classes.ResumeBuilderItems
{
    interface IResumeBuilderItem
    {
        string Name { get; }
        ResumeBuilderItemKind Kind { get; }
        string Value { get; set; }
    }

    enum ResumeBuilderItemKind : int
    {
        PersonalDetails = 0,
        Education = 1,
        WorkExperience = 2,
        MilitaryService = 3,
        Languages = 4
    }
}
