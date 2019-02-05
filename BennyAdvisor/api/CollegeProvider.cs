using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Caching;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public static class CollegeProvider
    {
        public static IEnumerable<CollegeModel> Get()
        {
            var cache = MemoryCache.Default;
            var colleges = cache["CollegeProvider"] as IEnumerable<CollegeModel>;

            if (colleges == null)
            {
                colleges = Load();

                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = new TimeSpan(12, 0, 0);
                cache.Set("CollegeProvider", colleges, policy);
            }

            return colleges;
        }

        static IEnumerable<CollegeModel> Load()
        {
            var bucket = new AwsBucket("info");
            var csv = bucket.ReadAllText($"college.csv");

            var colleges = new List<CollegeModel>();
            foreach (var line in Regex.Split(csv, "\r\n|\r|\n"))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(',');
                    if (parts.Length > 1)
                        colleges.Add(new CollegeModel() { Code = parts[0], Title = parts[1] });
                }
            }

            return colleges.OrderBy(c => c.Title);
        }
    }
}
