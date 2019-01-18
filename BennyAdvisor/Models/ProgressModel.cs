using System;

namespace BennyAdvisor.Models
{
    public class ProgressModel
    {
        public string Title { get; set; }
        public CourseStatus Status { get; set; }
        public int Credit { get; set; }
        public ProgressCourseModel Course { get; set; }
    }
}
