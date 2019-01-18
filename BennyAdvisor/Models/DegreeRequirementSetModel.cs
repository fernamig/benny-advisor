using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class DegreeRequirementSetModel
    {
        public string Title { get; set; }
        public IEnumerable<DegreeRequirementModel> Requirements { get; set; }
    }
}
