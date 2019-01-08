using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;

namespace BennyAdvisor.Pages.Advisor
{
    public class MyStudentsModel : PageModel
    {
        public IEnumerable<StudentModel> MyStudents { get; set; }

        public void OnGet()
        {
            MyStudents = Enumerable.Repeat(new StudentModel()
            {
                Level = "Graduate",
                ImagePath = "/images/test.jpg",
                FirstName = "Roger",
                LastName = "Montgomery",
                DegreeName = "ASSOC OF SCIENCE",
                DegreeCode = "AS1"
            }, 20);
        }
    }
}
