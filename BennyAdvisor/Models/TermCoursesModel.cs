using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class TermCoursesModel
    {
        public TermModel Term { get; set; }
        public IEnumerable<CourseGradeModel> Courses { get; set; }
    }
}
