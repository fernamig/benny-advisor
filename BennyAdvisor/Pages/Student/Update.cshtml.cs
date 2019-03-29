using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Pages.Student
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public StudentModel Student { get; set; }

        public void OnGet(string studentId)
        {
            var provider = new StudentProvider();
            Student = provider.TryGet(studentId) ?? new StudentModel() { Id = studentId, ImagePath = "/images/test.jpg" };
        }

        public IActionResult OnPost()
        {
            var provider = new StudentProvider();
            provider.Set(Student.Id, Student);
            return new RedirectToPageResult("Index");
        }
    }
}
