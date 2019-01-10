using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class CourseProvider : ROObjectProvider<CourseModel>
    {
        public CourseProvider()
            : base("course")
        {
        }

        public IEnumerable<CourseModel> GetAllTitles()
        {
            var csv = Bucket.ReadAllText($"all.csv");

            var courses = new List<CourseModel>();
            foreach (var line in Regex.Split(csv, "\r\n|\r|\n"))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(',');
                    courses.Add(new CourseModel() { Code = parts[0], Title = parts[1], Credit = 0 });
                }
            }

            return courses;
        }
    }
}
