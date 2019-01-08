using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;

namespace BennyAdvisor.Pages.Advisor.Student
{
    public class IndexModel : PageModel
    {
        public StudentModel Student { get; set; }

        public void OnGet()
        {
            Student = new StudentModel()
            {
                Level = "Graduate",
                ImagePath = "/images/test.jpg",
                FirstName = "Roger",
                LastName = "Montgomery",
                DegreeName = "ASSOC OF SCIENCE",
                DegreeCode = "AS1"
            };
        }
    }
}
