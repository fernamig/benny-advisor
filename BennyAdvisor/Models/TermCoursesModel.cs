using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class TermCoursesModel
    {
        public string TermTitle { get; set; }
        public int TermCode { get; set; }
        public IEnumerable<CourseGradeModel> Courses { get; set; }
    }
}
