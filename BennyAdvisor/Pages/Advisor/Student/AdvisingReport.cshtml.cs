using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Pages.Advisor.Student
{
    public class AdvisingReportModel : PageModel
    {
        public StudentModel Student { get; set; }

        public void OnGet(string studentId)
        {
            var provider = new StudentProvider();
            Student = provider.Get(studentId);
        }
    }
}
