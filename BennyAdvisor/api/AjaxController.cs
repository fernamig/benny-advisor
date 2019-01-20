using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BennyAdvisor.Models;
using BennyAdvisor.Reports;

namespace BennyAdvisor.api
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]/{id?}")]
    public class AjaxController : Controller
    {
        [HttpGet]
        public JsonResult GetAdvisorStudents(string id)
        {
            var advisorProvider = new AdvisorProvider();
            var studentProvider = new StudentProvider();

            var students = new List<StudentModel>();
            foreach (var sid in advisorProvider.Get(id))
            {
                students.Add(studentProvider.Get(sid));
            }

            return Json(students);
        }

        [HttpGet]
        public JsonResult GetCoursePlan(string id)
        {
            var provider = new CoursePlanProvider();
            return Json(provider.TryGet(id)?.Groups);
        }

        [HttpGet]
        public JsonResult GetRegistrarCourses(string id)
        {
            var provider = new RegistrarCourseProvider();
            return Json(provider.TryGet(id)?.Groups);
        }

        [HttpGet]
        public JsonResult GetAdvisingCoursePlan(string id)
        {
            var report = new CoursePlanReport();
            return Json(report.Generate(id));
        }

        [HttpGet]
        public JsonResult GetAdvisingTimelime(string id)
        {
            var report = new TimelimeReport();
            return Json(report.Generate(id));
        }

        [HttpGet]
        public JsonResult GetAdvisingProgress(string id)
        {
            var report = new ProgressReport();
            return Json(report.Generate(id));
        }

        [HttpGet]
        [Route("{advisorId}/{wk}")]
        public JsonResult GetAdvisingAvailability(string id, string advisorId, DateTime wk)
        {
            var report = new AvailabilityReport();
            return Json(report.Generate(advisorId, id, wk));
        }

        [HttpGet]
        public JsonResult GetCalendar(string id)
        {
            var provider = new CalendarProvider();
            return Json(provider.Get(id));
        }

        [HttpGet]
        public JsonResult GetStudentNotes(string id)
        {
            var provider = new NotesProvider();
            return Json(provider.Get(id));
        }

        [HttpPost]
        public JsonResult AddStudentNote(string id, [FromBody] NoteModel note)
        {
            note.Date = DateTime.UtcNow;
            note.Source = "BennyAdvisor";

            var provider = new NotesProvider();
            provider.Add(id, note);
            return Json("The note was added.");
        }
    }
}
