using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Reports
{
    public class ProgressReport
    {
        readonly CoursePlanReport Report = new CoursePlanReport();

        public List<ProgressCollectionModel> Generate(string studentId)
        {
            // Generate a dictionary of courses from the student course plan.
            var coursesProgress = new Dictionary<string, ProgressStatusModel>();
            foreach (var term in Report.Generate(studentId))
            {
                foreach (var c in term.Courses)
                {
                    coursesProgress[c.Code] = new ProgressStatusModel(c, term.Term);
                }
            }
            var courseIds = coursesProgress.Keys.ToHashSet();

            // TODO: Load the requirements for the student degree. For now use hardcoded CS requirements.
            var provider = new DegreeRequirementProvider(); 
            var requirements = provider.Get("cs");

            // Find the matching courses for each requirement.
            var matches = new Dictionary<string, MatchingCourseRequirementModel>();
            foreach (var set in requirements)
            {
                foreach (var req in set.Requirements)
                {
                    var matchingCourses = GetMatchingCourses(courseIds, req);
                    if (matchingCourses.Count > 0)
                    {
                        matches[$"{set.Title}/{req.Title}"] =
                            new MatchingCourseRequirementModel()
                            {
                                Requirement = req,
                                Courses = matchingCourses
                            };
                    }
                }
            }
            matches = TrimMatches(matches);

            return Process(matches, coursesProgress, requirements);
        }

        List<ProgressCollectionModel> Process(
            Dictionary<string, MatchingCourseRequirementModel> matches,
            Dictionary<string, ProgressStatusModel> coursesProgress,
            DegreeRequirementSetModel[] requirements)
        {
            var progress = new List<ProgressCollectionModel>();
            foreach (var set in requirements)
            {
                var setProgress = new List<ProgressModel>();
                var setStatus = CourseStatus.Completed;

                foreach (var req in set.Requirements)
                {
                    var key = $"{set.Title}/{req.Title}";
                    int totalCredits = 0;
                    int totalCourses = 0;

                    if (matches.ContainsKey(key))
                    {
                        var matchingCourses = matches[key];

                        foreach (var c in matchingCourses.Courses)
                        {
                            var course = coursesProgress[c];
                            setProgress.Add(new ProgressModel()
                            {
                                Title = req.Title,
                                Status = course.Status,
                                Credit = course.Credit,
                                Course = new ProgressCourseModel()
                                {
                                    Code = course.Code,
                                    Title = course.Title,
                                    Grade = course.Grade,
                                    Term = course.Term,
                                }
                            });
                            totalCredits += course.Credit;
                            totalCourses++;

                            if (course.Status < setStatus)
                                setStatus = course.Status;
                        }
                    }

                    // Check if the requirement has been fully met.
                    if ((totalCredits < req.MinCredits) ||
                        (totalCourses < req.MinCount))
                    {
                        // The requirement has not been fully met so add the remaining
                        // requirements to the progress.
                        setProgress.Add(new ProgressModel()
                        {
                            Title = req.Title,
                            Status = CourseStatus.NotCompleted,
                            Credit = req.MinCredits - totalCredits,
                        });
                        setStatus = CourseStatus.NotCompleted;
                    }
                }

                progress.Add(new ProgressCollectionModel()
                {
                    Title = set.Title,
                    Status = setStatus,
                    Credit = setProgress.Sum(x => x.Credit),
                    Requirements = setProgress
                });
            }

            return progress;
        }

        Dictionary<string, MatchingCourseRequirementModel> TrimMatches(Dictionary<string, MatchingCourseRequirementModel> matches)
        {
            var matchingCourses = new Dictionary<string, MatchingCourseRequirementModel>();

            while (matches.Count > 0)
            {
                var kv = matches.OrderBy(x => x.Value.Courses.Count).First();
                matches.Remove(kv.Key);

                // Take into account the maximum credits and maximum courses.
                // TODO: We have to take into account the maximum credits and maximum courses.
                kv.Value.Courses = kv.Value.Courses.Take(kv.Value.Requirement.MaxCount).ToHashSet();

                matchingCourses[kv.Key] = kv.Value;

                foreach (var m in matches.Values)
                    m.Courses.RemoveWhere(x => kv.Value.Courses.Contains(x));
            }

            return matchingCourses;
        }

        HashSet<string> GetMatchingCourses(HashSet<string> courses, DegreeRequirementModel req)
        {
            var matchingCourses = new HashSet<string>();

            foreach (var id in req.Allow)
            {
                var matches = GetMatchingCourses(courses, id);
                matches.RemoveWhere(x => req.Deny.Contains(x));
                matchingCourses.UnionWith(matches);
            }

            return matchingCourses;
        }

        HashSet<string> GetMatchingCourses(HashSet<string> courses, string req)
        {
            var matches = new HashSet<string>();

            if (req.Contains('#'))
            {
                var regex = new Regex($"^{req.Replace("#", "[0-9]")}$");

                foreach (var cid in courses)
                {
                    if (regex.IsMatch(cid))
                        matches.Add(cid);
                }
            }
            else if (courses.Contains(req))
            {
                matches.Add(req);
            }

            return matches;
        }

        class ProgressStatusModel : ProgressCourseModel
        {
            public int Credit { get; set; }
            public CourseStatus Status { get; set; }

            public ProgressStatusModel(CourseGradeModel c, TermModel term)
            {
                Code = c.Code;
                Title = c.Title;
                Credit = c.Credit;
                Status = c.Status;
                Grade = c.Grade;
                Term = term;
            }
        }
        class MatchingDegreeRequirementModel
        {
            public DegreeRequirementModel Requirement { get; set; }
            public HashSet<string> MatchingCourses { get; set; }
        }
        class MatchingCourseRequirementSetModel
        {
            public string Title { get; set; }
            public IEnumerable<MatchingCourseRequirementModel> Requirements { get; set; }
        }
        class MatchingCourseRequirementModel
        {
            public DegreeRequirementModel Requirement { get; set; }
            public HashSet<string> Courses { get; set; }
        }
    }
}
