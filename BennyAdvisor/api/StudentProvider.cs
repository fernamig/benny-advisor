using System;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class StudentProvider : ObjectProvider<StudentModel>
    {
        public StudentProvider()
            : base("student")
        {
        }
    }
}
