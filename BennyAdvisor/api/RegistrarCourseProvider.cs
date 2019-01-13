using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class RegistrarCourseProvider : GroupProvider<EnrollmentModel>
    {
        public RegistrarCourseProvider()
            : base("registrar-course")
        {
        }
    }
}
