using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class CoursePlanProvider : GroupProvider<string>
    {
        public CoursePlanProvider()
            : base("course-plan")
        {
        }
    }
}
