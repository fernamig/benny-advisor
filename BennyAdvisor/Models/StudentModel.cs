using System;
using System.Collections.Generic;
using System.Linq;

namespace BennyAdvisor.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string ImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DegreeName { get; set; }
        public string DegreeCode { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string OverallGpa { get; set; }
        public bool Confidential { get; set; }
    }
}
