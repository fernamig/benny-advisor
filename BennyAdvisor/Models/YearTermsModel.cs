using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class YearTermsModel
    {
        public int Year { get; set; }
        public IEnumerable<TermCoursesModel> Terms { get; set; }

        public YearTermsModel()
        {
        }
        public YearTermsModel(int year, IEnumerable<TermCoursesModel> terms)
        {
            Year = year;
            Terms = terms;
        }
    }
}
