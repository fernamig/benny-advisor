using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Reports
{
    public class CoursePlanReport
    {
        public List<TermCoursesModel> Generate(string studentId)
        {
            // TODO: Where do we get the current term code from?
            const int currentTermCode = 20191;
            var termTitles = new Dictionary<int, string>();
            var terms = new Dictionary<int, Dictionary<string, CourseGradeModel>>();

            // Get a collection of all of the courses on the student course plan.
            GetStudentCoursePlan(terms, termTitles, studentId);

            // Get the courses that the student is registered for and
            // merge that with the courses in the student course plan.
            MergeCourseGrades(terms, termTitles, currentTermCode, studentId);

            // Update the status of each course.
            UpdateStatus(terms, currentTermCode);

            // Now generate the course plan report.
            return GeneratePlan(terms, termTitles);
        }

        void GetStudentCoursePlan(
            Dictionary<int, Dictionary<string, CourseGradeModel>> terms,
            Dictionary<int, string> termTitles, string studentId)
        {
            var provider = new CoursePlanProvider();

            var group = provider.TryGet(studentId);
            if (group != null)
            {
                foreach (var g in group.Groups)
                {
                    var termCode = int.Parse(g.Id);
                    termTitles[termCode] = g.Title;
                    terms[termCode] = g.Members
                        .Select(x => new CourseGradeModel() { Code = x })
                        .ToDictionary(x => x.Code, x => x);
                }
            }
        }

        void MergeCourseGrades(
            Dictionary<int, Dictionary<string, CourseGradeModel>> terms,
            Dictionary<int, string> termTitles,
            int currentTermCode, string studentId)
        {
            var provider = new RegistrarCourseProvider();

            var group = provider.TryGet(studentId);
            if (group != null)
            {
                foreach (var g in group.Groups)
                {
                    var termCode = int.Parse(g.Id);
                    termTitles[termCode] = g.Title;

                    if (!terms.ContainsKey(termCode))
                        terms[termCode] = new Dictionary<string, CourseGradeModel>();
                    var courses = terms[termCode];

                    foreach (var c in g.Members)
                    {
                        if (!courses.ContainsKey(c.Course))
                        {
                            courses[c.Course] = new CourseGradeModel()
                            {
                                Code = g.Id,
                            };
                        }

                        if (termCode < currentTermCode)
                            courses[c.Course].Grade = c.Grade;
                        else
                            courses[c.Course].Grade = -1;
                    }
                }
            }
        }

        void UpdateStatus(
            Dictionary<int, Dictionary<string, CourseGradeModel>> terms,
            int currentTermCode)
        {
            foreach (var kv in terms)
            {
                var termCode = kv.Key;

                foreach (var course in kv.Value.Values)
                {
                    if (termCode < currentTermCode)
                    {
                        course.Status = course.Grade > 0
                            ? CourseStatus.Completed : CourseStatus.Skipped;
                    }
                    else
                    {
                        course.Status = (termCode ==
                            currentTermCode ? CourseStatus.InProgress : CourseStatus.Planned);
                    }
                }
            }
        }

        List<TermCoursesModel> GeneratePlan(
            Dictionary<int, Dictionary<string, CourseGradeModel>> terms,
            Dictionary<int, string> termTitles)
        {
            var provider = new CourseProvider();

            var plan = new List<TermCoursesModel>();
            foreach (var kv in terms)
            {
                foreach (var c in kv.Value.Values)
                {
                    var info = provider.Get(c.Code);
                    if (info != null)
                    {
                        c.Credit = info.Credit;
                        c.Title = info.Title;
                    }
                }

                plan.Add(new TermCoursesModel()
                {
                    TermCode = kv.Key,
                    TermTitle = termTitles[kv.Key],
                    Courses = kv.Value.Values
                });
            }

            return plan;
        }
    }
}
