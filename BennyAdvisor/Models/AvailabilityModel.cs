using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class AvailabilityModel
    {
        public DateTime Date { get; set; }
        public IEnumerable<SlotStatus> Slots { get; set; }
    }

}
