using System;

namespace BennyAdvisor.Models
{
    public class CourseGradeModel : CourseModel
    {
        public CourseStatus Status { get; set; }
        public double Grade { get; set; }
    }
}
