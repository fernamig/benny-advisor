using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class AvailabilityDayModel
    {
        public DateTime Day { get; set; }
        public IEnumerable<TimeRange> Slots { get; set; }
    }

    public class AvailabilityModel
    {
        public int WeeksFromToday { get; set; }
        public DateTime Earliest { get; set; }
        public DateTime Latest { get; set; }
        public IEnumerable<AvailabilityDayModel> Days { get; set; }
    }
}
