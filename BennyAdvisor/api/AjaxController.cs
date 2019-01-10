using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]/{id?}")]
    public class AjaxController : Controller
    {
        [HttpGet]
        public JsonResult GetAdvisorStudents(string id)
        {
            var ap = new AdvisorProvider();
            var sp = new StudentProvider();

            var students = new List<StudentModel>();
            foreach (var sid in ap.Get(id))
            {
                students.Add(sp.Get(sid));
            }

            return Json(students);
        }
    }
}