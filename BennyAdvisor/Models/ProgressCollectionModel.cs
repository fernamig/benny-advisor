using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class ProgressCollectionModel
    {
        public string Title { get; set; }
        public CourseStatus Status { get; set; }
        public int Credit { get; set; }
        public IEnumerable<ProgressModel> Requirements { get; set; }
    }
}
