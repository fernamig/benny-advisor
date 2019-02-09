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
        static readonly object s_LockCoursePlan = new object();
        static readonly object s_LockNotes = new object();

        [HttpGet]
        public JsonResult GetCourses()
        {
            var provider = new CourseProvider();
            return Json(provider.GetAllTitles());
        }

        [HttpGet]
        public JsonResult GetCourseData(string id)
        {
            var provider = new CourseProvider();
            return Json(provider.Get(id));
        }

        [HttpGet]
        public JsonResult GetAdvisorStudents(string id)
        {
            return Json(GetStudents(id));
        }

        [HttpGet]
        public JsonResult GetAdvisorStudentAlerts(string id)
        {
            var provider = new StudentClaimsProvider();
            var results = new List<StudentClaimsModel>();
            foreach (var s in GetStudents(id))
            {
                var claims = provider.HasClaims(s.Id, "HSRCAssistance");
                if (claims.Any())
                {
                    results.Add(new StudentClaimsModel()
                    {
                        Student = s,
                        Claims = claims
                    });
                }
            }
            return Json(results);
        }

        [HttpGet]
        public JsonResult GetMyProfileScheduler(string id)
        {
            var provider = new MyProfileProvider();
            var my = provider.TryGet(id) ?? new MyProfileModel();
            return Json(new MyProfileSchedulerViewModel
            {
                Limits = my.Scheduler.Limits,
                Availability = new MyProfileSchedulerAvailabilityViewModel(my.Scheduler.Availability)
            });
        }

        [HttpPost]
        public JsonResult SetMyProfileScheduler(string id, [FromBody] MyProfileSchedulerViewModel schedulerView)
        {
            var provider = new MyProfileProvider();
            var my = provider.TryGet(id) ?? new MyProfileModel();
            my.Scheduler = new MyProfileSchedulerModel
            {
                Limits = schedulerView.Limits,
                Availability = new MyProfileSchedulerAvailabilityModel(schedulerView.Availability),
            };
            provider.Set(id, my);

            return Json("The scheduler preferences have been updated.");
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
            lock (s_LockNotes)
            {
                note.Date = DateTime.UtcNow;
                note.Source = "BennyAdvisor";

                var provider = new NotesProvider();
                provider.Add(id, note);
                return Json("The note was added.");
            }
        }

        [HttpGet]
        public JsonResult GetStudentTestScores(string id)
        {
            var provider = new TestScoresProvider();
            return Json(provider.Get(id));
        }

        [HttpPost]
        public JsonResult SetCoursePlan(string id, [FromBody] List<GroupModel<string>> plan)
        {
            lock (s_LockCoursePlan)
            {
                var provider = new CoursePlanProvider();
                provider.Set(id, plan);
                return Json("The plan has been updated.");
            }
        }

        [HttpGet]
        [Route("{claims}")]
        public JsonResult GetStudentClaims(string id, string claims)
        {
            var provider = new StudentClaimsProvider();
            return Json(provider.HasClaims(id, claims));
        }

        private IEnumerable<StudentModel> GetStudents(string advisorId)
        {
            var advisorProvider = new AdvisorProvider();
            var studentProvider = new StudentProvider();

            var students = new List<StudentModel>();
            foreach (var sid in advisorProvider.Get(advisorId))
            {
                students.Add(studentProvider.Get(sid));
            }

            return students;
        }
    }
}
