using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BennyAdvisor.Models;

namespace BennyAdvisor.Pages.Advisor
{
    public class IndexModel : PageModel
    {
        public IEnumerable<StudentModel> FlaggedStudents { get; set; }
        public IEnumerable<AppointmentModel> UpcomingAppointments { get; set; }
        public IEnumerable<AppointmentModel> RecentAppointments { get; set; }

        public void OnGet(string advisorId)
        {
            UpcomingAppointments = Enumerable.Repeat(new AppointmentModel()
            {
                DateTime = "Wednesday, August 1 at 9 am",
                Agenda = "Degree review",
                Student = new StudentModel()
                {
                    Level = "Graduate",
                    ImagePath = "/images/test.jpg",
                    FirstName = "Roger",
                    LastName = "Montgomery",
                    DegreeName = "ASSOC OF SCIENCE",
                    DegreeCode = "AS1"
                }
            }, 4);
            RecentAppointments = Enumerable.Repeat(new AppointmentModel()
            {
                DateTime = "Wednesday, August 1 at 9 am",
                Agenda = "Degree review",
                Student = new StudentModel()
                {
                    Level = "Graduate",
                    ImagePath = "/images/test.jpg",
                    FirstName = "Roger",
                    LastName = "Montgomery",
                    DegreeName = "ASSOC OF SCIENCE",
                    DegreeCode = "AS1"
                }
            }, 4);
            FlaggedStudents = Enumerable.Repeat(new StudentModel()
            {
                Level = "Graduate",
                ImagePath = "/images/test.jpg",
                FirstName = "Roger",
                LastName = "Montgomery",
                DegreeName = "ASSOC OF SCIENCE",
                DegreeCode = "AS1"
            }, 4);
        }
    }
}
