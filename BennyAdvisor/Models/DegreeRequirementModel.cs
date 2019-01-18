using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class DegreeRequirementModel
    {
        public string Title { get; set; }
        public int MinCredits { get; set; }
        public int MaxCredits { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public HashSet<string> Allow { get; set; }
        public HashSet<string> Deny { get; set; }
    }
}
