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
            // TODO: Get these from advisor preferences.
            var inc = new TimeSpan(0, 30, 0);
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(16, 0, 0);
            var hoursAdvance = 0; // TODO: Time in advance.
            var daysFuture = 3 * 7;
            var earliest = DateTime.Today.ToUniversalTime().AddDays(2).StartOfWeek(DayOfWeek.Monday);
            var weeksFromToday = (day - earliest).Days / 7;

            var advisorEvents = Provider.Get(advisorId).ToList();
            var studentEvents = Provider.Get(studentId).ToList();

            var days = new List<DateTime>();
            var daySlots = new List<List<TimeSlot>>();
            for (int i = 0; i < 5; i++, day = day.AddDays(1))
            {
                // Generate the advisor time slots for the day.
                var timeSlots = GenerateTimeSlots(day, start, end, inc);
                // Fill in the slots that are in use.
                SetSlotsStatus(timeSlots, studentEvents, SlotStatus.InClass);
                SetSlotsStatus(timeSlots, advisorEvents, SlotStatus.Unavailable);

                days.Add(day);
                daySlots.Add(timeSlots);
            }

            var slotCount = daySlots[0].Count;
            var availability = new List<AvailabilitySlotModel>();
            for (int i = 0; i < slotCount; i++)
            {
                availability.Add(new AvailabilitySlotModel()
                {
                    Start = daySlots[0][i].End - inc,
                    End = daySlots[0][i].End,
                    Slots = new SlotStatus[] {
                        daySlots[0][i].Status,
                        daySlots[1][i].Status,
                        daySlots[2][i].Status,
                        daySlots[3][i].Status,
                        daySlots[4][i].Status,
                    }
                });
            }

            return new AvailabilityModel() {
                WeeksFromToday = weeksFromToday,
                Earliest = earliest,
                Latest = earliest.AddDays(daysFuture),
                Days = days,
                Availability = availability
            };
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
                timeSlots.Add(new TimeSlot()
                {
                    End = t,
                    Status = SlotStatus.Available
                });
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

        class TimeSlot
        {
            public DateTime End { get; set; }
            public SlotStatus Status { get; set; }
        }
    }
}
