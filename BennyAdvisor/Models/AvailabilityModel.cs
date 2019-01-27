using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class AvailabilitySlotModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<SlotStatus> Slots { get; set; }
    }

    public class AvailabilityModel
    {
        public int WeeksFromToday { get; set; }
        public DateTime Earliest { get; set; }
        public DateTime Latest { get; set; }
        public IEnumerable<DateTime> Days { get; set; }
        public IEnumerable<AvailabilitySlotModel> Availability { get; set; }
    }
}
