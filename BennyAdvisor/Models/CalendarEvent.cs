using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Dictionary<string, string> Meta { get; set; }
    }
}
