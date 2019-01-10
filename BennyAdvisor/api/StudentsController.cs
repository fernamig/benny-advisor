using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    [Produces("application/json")]
    [Route("api/[controller]/{id}")]
    public class StudentsController : Controller
    {
        [HttpGet]
        public JsonResult OnGet(string id)
        {
            var provider = new StudentProvider();
            return Json(new 
            {
                data = provider.Get(id),
                links = new
                {
                    self = $"https://api.oregonstate.edu/v1/students/{id}"
                }
            });
        }
    }
}
