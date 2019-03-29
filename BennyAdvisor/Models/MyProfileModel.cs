using System;
using System.Collections.Generic;
using System.Linq;

namespace BennyAdvisor.Models
{
    // TODO: split these into individual files.
    public class MyProfileModel
    {
        public ScheduleSettingsModel Schedule { get; set; }

        public MyProfileModel()
        {
            Schedule = new ScheduleSettingsModel();
        }
    }

    public class ScheduleSettingsModel
    {
        public ScheduleSettingsTimeTradeModel TimeTrade { get; set; }
        public ScheduleSettingsBuiltinModel Builtin { get; set; }
    }
    public class ScheduleSettingsTimeTradeModel
    {
        public string Url { get; set; }
    }
    public class ScheduleSettingsBuiltinModel
    {
        public ScheduleLimitsModel Limits { get; set; }
        public ScheduleAvailabilityViewModel Availability { get; set; }
    }
    public class ScheduleBuiltinModel
    {
        public ScheduleLimitsModel Limits { get; set; }
        public ScheduleAvailabilityModel Availability { get; set; }

        public ScheduleBuiltinModel(ScheduleSettingsBuiltinModel m)
        {
            Limits = m.Limits;
            Availability = new ScheduleAvailabilityModel(m.Availability);
        }
    }
    public class ScheduleAvailabilityReportModel
    {
        public ScheduleSettingsTimeTradeModel TimeTrade { get; set; }
        public AvailabilityModel Builtin { get; set; }
    }
    public class ScheduleAvailabilityModel
    {
        public IEnumerable<ScheduleRange> Monday { get; set; }
        public IEnumerable<ScheduleRange> Tuesday { get; set; }
        public IEnumerable<ScheduleRange> Wednesday { get; set; }
        public IEnumerable<ScheduleRange> Thursday { get; set; }
        public IEnumerable<ScheduleRange> Friday { get; set; }

        public ScheduleAvailabilityModel() { }
        public ScheduleAvailabilityModel(ScheduleAvailabilityViewModel m)
        {
            Monday = Parse(m.Monday);
            Tuesday = Parse(m.Tuesday);
            Wednesday = Parse(m.Wednesday);
            Thursday = Parse(m.Thursday);
            Friday = Parse(m.Friday);
        }

        IEnumerable<ScheduleRange> Parse(string str)
        {
            var times = new List<ScheduleRange>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                foreach (var range in str.Split(','))
                {
                    var parts = range.Split('-');
                    if (parts.Length == 2)
                    {
                        times.Add(new ScheduleRange
                        {
                            Start = DateTimeOffset.Parse(parts[0]).TimeOfDay,
                            End = DateTimeOffset.Parse(parts[1]).TimeOfDay
                        });
                    }
                }
            }
            return times;
        }
    }
    public class ScheduleAvailabilityViewModel
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
    }
    public class ScheduleLimitsModel
    {
        public int MinHours { get; set; }
        public int MaxDays { get; set; }
        public int AppointmentLength { get; set; }
    }
    public class ScheduleRange
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
