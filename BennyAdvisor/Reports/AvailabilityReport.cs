using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.api;
using BennyAdvisor.Models;
using BennyAdvisor.Extensions;

namespace BennyAdvisor.Reports
{
    public class AvailabilityReport
    {
        readonly CalendarProvider Provider = new CalendarProvider();

        public AvailabilityModel Generate(string advisorId, string studentId, DateTime day)
        {
            var provider = new MyProfileProvider();
            var sch = provider.GetSchedule(advisorId);

            var apptLen = new TimeSpan(0, sch.Limits.AppointmentLength, 0);
            var earliest = DateTime.Today.ToUniversalTime().AddDays(2).StartOfWeek(DayOfWeek.Monday);
            var weeksFromToday = (day - earliest).Days / 7;
            var minDate = DateTime.Now.AddHours(sch.Limits.MinHours);

            var advisorEvents = Provider.Get(advisorId).ToList();
            var studentEvents = Provider.Get(studentId).ToList();

            var days = new List<AvailabilityDayModel>();
            days.Add(new AvailabilityDayModel()
            {
                Day = day,
                Slots = GetAvailableSlots(advisorEvents, studentEvents, day, apptLen, minDate, sch.Availability.Monday)
            });
            day = day.AddDays(1);
            days.Add(new AvailabilityDayModel()
            {
                Day = day,
                Slots = GetAvailableSlots(advisorEvents, studentEvents, day, apptLen, minDate, sch.Availability.Tuesday)
            });
            day = day.AddDays(1);
            days.Add(new AvailabilityDayModel()
            {
                Day = day,
                Slots = GetAvailableSlots(advisorEvents, studentEvents, day, apptLen, minDate, sch.Availability.Wednesday)
            });
            day = day.AddDays(1);
            days.Add(new AvailabilityDayModel()
            {
                Day = day,
                Slots = GetAvailableSlots(advisorEvents, studentEvents, day, apptLen, minDate, sch.Availability.Thursday)
            });
            day = day.AddDays(1);
            days.Add(new AvailabilityDayModel()
            {
                Day = day,
                Slots = GetAvailableSlots(advisorEvents, studentEvents, day, apptLen, minDate, sch.Availability.Friday)
            });

            return new AvailabilityModel() {
                WeeksFromToday = weeksFromToday,
                Earliest = earliest,
                Latest = earliest.AddDays(sch.Limits.MaxDays),
                Days = days
            };
        }

        IEnumerable<TimeRange> GetAvailableSlots(
            List<CalendarEvent> advisorEvents, List<CalendarEvent> studentEvents,
            DateTime day, TimeSpan apptLen, DateTime minDate,
            IEnumerable<AvailabilityPreferencesRange> ranges)
        {
            var available = new List<TimeSlot>();

            foreach (var r in ranges)
            {
                // Generate the advisor time slots for the day.
                var timeSlots = GenerateTimeSlots(day, r.Start, r.End, apptLen);
                // Union the slots to removed overlapping slots.
                available = Union(available, timeSlots);
            }

            // Fill in the slots that are in use.
            SetSlotsStatus(available, studentEvents, SlotStatus.InClass);
            SetSlotsStatus(available, advisorEvents, SlotStatus.Unavailable);

            return available
                .Where(x => x.Status == SlotStatus.Available)
                .Where(x => x.End > minDate)
                .Select(x => new TimeRange { Start = x.Start, End = x.End })
                .OrderBy(x => x.Start);
        }

        List<TimeSlot> GenerateTimeSlots(DateTime day, TimeSpan start, TimeSpan end, TimeSpan len)
        {
            var startTime = new DateTime(day.Year, day.Month, day.Day,
                start.Hours, start.Minutes, start.Seconds);
            var endTime = new DateTime(day.Year, day.Month, day.Day,
                end.Hours, end.Minutes, end.Seconds);

            var timeSlots = new List<TimeSlot>();
            for (DateTime t = startTime + len; t <= endTime; t += len)
            {
                timeSlots.Add(new TimeSlot(t - len, t));
            }
            return timeSlots;
        }

        List<TimeRange> GenerateTimeRanges(DateTime day, TimeSpan start, TimeSpan end, TimeSpan len)
        {
            var startTime = new DateTime(day.Year, day.Month, day.Day,
                start.Hours, start.Minutes, start.Seconds);
            var endTime = new DateTime(day.Year, day.Month, day.Day,
                end.Hours, end.Minutes, end.Seconds);

            var ranges = new List<TimeRange>();
            for (DateTime t = startTime + len; t <= endTime; t += len)
            {
                ranges.Add(new TimeRange()
                {
                    End = t,
                    Start = t - len
                });
            }
            return ranges;
        }

        void SetSlotsStatus(List<TimeSlot> timeSlots, List<CalendarEvent> times, SlotStatus status)
        {
            for (int i = 0, j = 0; (i < timeSlots.Count) && (j < times.Count); i++)
            {
                while (timeSlots[i].End > times[j].End)
                {
                    j++;
                    if (j >= times.Count)
                        return;
                }

                if (timeSlots[i].End > times[j].Start)
                {
                    timeSlots[i].Status = status;
                }
            }
        }

        List<TimeSlot> Union(List<TimeSlot> slots1, List<TimeSlot> slots2)
        {
            List<TimeSlot> slots = new List<TimeSlot>();

            foreach (var t in slots1)
            {
                if (!slots.Any(x => x.Overlaps(t)))
                    slots.Add(t);
            }
            foreach (var t in slots2)
            {
                if (!slots.Any(x => x.Overlaps(t)))
                    slots.Add(t);
            }

            return slots;
        }

        class TimeSlot
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public SlotStatus Status { get; set; }

            public TimeSlot(DateTime start, DateTime end)
            {
                Start = start;
                End = end;
                Status = SlotStatus.Available;
            }

            public bool Overlaps(TimeSlot t)
            {
                if ((Start == t.Start) && (End == t.End))
                    return true;
                if ((Start < t.Start) && (t.Start < End))
                    return true;
                if ((Start < t.End) && (t.End < End))
                    return true;
                return false;
            }
        }
    }
}
