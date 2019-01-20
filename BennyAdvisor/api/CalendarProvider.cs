using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class CalendarProvider
    {
        readonly CollectionProvider<CalendarEvent> Provider;

        public CalendarProvider()
        {
            Provider = new CollectionProvider<CalendarEvent>("calendar");
        }

        public IEnumerable<CalendarEvent> Get(string id)
        {
            return Get(id, DateTime.MinValue, DateTime.MaxValue);
        }
        public IEnumerable<CalendarEvent> Get(string id, DateTime start, DateTime end)
        {
            return Provider.Get(id)
                .Where(x => (start <= x.Start) && (x.End <= end))
                .OrderBy(x => x.Start);
        }

        public void Set(string id, IEnumerable<CalendarEvent> ev)
        {
            Provider.Set(id, ev);
        }

        public void Add(string id, CalendarEvent ev)
        {
            Provider.Set(id, Provider.Get(id).Concat(new[] { ev }));
        }
    }
}
