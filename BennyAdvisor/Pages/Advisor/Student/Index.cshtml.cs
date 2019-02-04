using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Pages.Advisor.Student
{
    public class IndexModel : PageModel
    {
        public StudentModel Student { get; set; }

        public void OnGet(string studentId)
        {
            var provider = new StudentProvider();
            Student = provider.TryGet(studentId);
        }
    }
}
