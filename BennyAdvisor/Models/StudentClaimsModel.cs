using System;
using System.Collections.Generic;
using System.Linq;

namespace BennyAdvisor.Models
{
    public class StudentClaimsModel
    {
        public StudentModel Student { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
