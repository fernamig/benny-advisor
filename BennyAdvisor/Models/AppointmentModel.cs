using System;

namespace BennyAdvisor.Models
{
    public class AppointmentModel
    {
        public string DateTime { get; set; }
        public string Agenda { get; set; }
        public StudentModel Student { get; set; }
    }
}
