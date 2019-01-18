using System;

namespace BennyAdvisor.Models
{
    public class ProgressCourseModel
    {
        public double Grade { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public TermModel Term { get; set; }
    }
}
