using System;
using System.Collections.Generic;
using System.Linq;

namespace BennyAdvisor.Models
{
    public class MyProfileModel
    {
        public MyProfileSchedulerModel Scheduler { get; set; }

        public MyProfileModel()
        {
            Scheduler = new MyProfileSchedulerModel();
        }
    }

    public class MyProfileSchedulerViewModel
    {
        public MyProfileSchedulerLimitsModel Limits { get; set; }
        public MyProfileSchedulerAvailabilityViewModel Availability { get; set; }
    }
    public class MyProfileSchedulerAvailabilityViewModel
    {
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }

        public MyProfileSchedulerAvailabilityViewModel() { }
        public MyProfileSchedulerAvailabilityViewModel(MyProfileSchedulerAvailabilityModel m)
        {
            Monday = string.Join(",", m.Monday.Select(x => $"{x.Start.ToString(@"h\:mm")}-{x.End.ToString(@"h\:mm")}"));
            Tuesday = string.Join(",", m.Tuesday.Select(x => $"{x.Start.ToString(@"h\:mm")}-{x.End.ToString(@"h\:mm")}"));
            Wednesday = string.Join(",", m.Wednesday.Select(x => $"{x.Start.ToString(@"h\:mm")}-{x.End.ToString(@"h\:mm")}"));
            Thursday = string.Join(",", m.Thursday.Select(x => $"{x.Start.ToString(@"h\:mm")}-{x.End.ToString(@"h\:mm")}"));
            Friday = string.Join(",", m.Friday.Select(x => $"{x.Start.ToString(@"h\:mm")}-{x.End.ToString(@"h\:mm")}"));
        }
    }
    public class MyProfileSchedulerLimitsModel
    {
        public int MinHours { get; set; }
        public int MaxDays { get; set; }
        public int AppointmentLength { get; set; }
    }
    public class MyProfileSchedulerModel
    {
        public MyProfileSchedulerLimitsModel Limits { get; set; }
        public MyProfileSchedulerAvailabilityModel Availability { get; set; }

        public MyProfileSchedulerModel()
        {
            Limits = new MyProfileSchedulerLimitsModel
            {
                MinHours = 12,
                MaxDays = 21,
                AppointmentLength = 30,
            };
            Availability = new MyProfileSchedulerAvailabilityModel();
         }
    }
    public class MyProfileSchedulerAvailabilityModel
    {
        public IEnumerable<AvailabilityPreferencesRange> Monday { get; set; }
        public IEnumerable<AvailabilityPreferencesRange> Tuesday { get; set; }
        public IEnumerable<AvailabilityPreferencesRange> Wednesday { get; set; }
        public IEnumerable<AvailabilityPreferencesRange> Thursday { get; set; }
        public IEnumerable<AvailabilityPreferencesRange> Friday { get; set; }

        public MyProfileSchedulerAvailabilityModel() { }
        public MyProfileSchedulerAvailabilityModel(MyProfileSchedulerAvailabilityViewModel m)
        {
            Monday = Parse(m.Monday);
            Tuesday = Parse(m.Tuesday);
            Wednesday = Parse(m.Wednesday);
            Thursday = Parse(m.Thursday);
            Friday = Parse(m.Friday);
        }

        IEnumerable<AvailabilityPreferencesRange> Parse(string str)
        {
            var times = new List<AvailabilityPreferencesRange>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                foreach (var range in str.Split(','))
                {
                    var parts = range.Split('-');
                    if (parts.Length == 2)
                    {
                        times.Add(new AvailabilityPreferencesRange
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
    public class AvailabilityPreferencesRange
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
