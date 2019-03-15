using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.api;

namespace BennyAdvisor.Pages.Service
{
    public class StudentModel : PageModel
    {
        public Models.StudentModel Student { get; set; }

        public void OnGet(string studentId)
        {
            var provider = new StudentProvider();
            Student = provider.TryGet(studentId);
        }
    }
}
