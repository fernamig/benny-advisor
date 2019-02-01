using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;
using BennyAdvisor.api;

namespace BennyAdvisor.Reports
{
    public class TimelimeReport
    {
        readonly CoursePlanReport Report = new CoursePlanReport();

        public IEnumerable<YearTermsModel> Generate(string studentId)
        {
            var terms = Report.Generate(studentId);

            // Group the terms into years.
            var groups = terms
                .GroupBy(x => (x.Term.Code - 3) / 10)
                .OrderBy(x => x.Key)
                .Select(x => new YearTermsModel(x.Key, x))
                .ToList();

            // Add 3 years (or 4 years) to the end to reduce the complexity of the below code.
            if (groups.Count == 0)
                InsertYears(groups, DateTime.Now.Year, groups.Count, 4);
            else
                InsertYears(groups, groups[groups.Count - 1].Year, groups.Count, 3);

            // Add any missing years.
            var count = Math.Min(3, groups[1].Year - groups[0].Year - 1);
            if (count > 0)
                InsertYears(groups, groups[0].Year, 1, count);

            count = Math.Min(2, groups[2].Year - groups[1].Year - 1);
            if (count > 0)
                InsertYears(groups, groups[1].Year, 2, count);

            count = Math.Min(1, groups[3].Year - groups[2].Year - 1);
            if (count > 0)
                InsertYears(groups, groups[2].Year, 3, count);

            // Take only the first 4 years for the timeline.
            return groups.Take(4);
        }

        void InsertYears(List<YearTermsModel> groups, int year, int index, int count)
        {
            year += count;
            for (int i = 0; i < count; i++, year--)
            {
                int code;
                string title;

                var terms = new Dictionary<int, TermCoursesModel>();
                title = $"{year} Fall";
                code = CalcTermCode(title);
                terms[code] = new TermCoursesModel()
                {
                    Term = new TermModel() { Code = code, Title = title },
                    Courses = Enumerable.Empty<CourseGradeModel>()
                };
                title = $"{year + 1} Winter";
                code = CalcTermCode(title);
                terms[code] = new TermCoursesModel()
                {
                    Term = new TermModel() { Code = code, Title = title },
                    Courses = Enumerable.Empty<CourseGradeModel>()
                };
                title = $"{year + 1} Spring";
                code = CalcTermCode(title);
                terms[code] = new TermCoursesModel()
                {
                    Term = new TermModel() { Code = code, Title = title },
                    Courses = Enumerable.Empty<CourseGradeModel>()
                };

                groups.Insert(index, new YearTermsModel(year, terms.Values.OrderBy(x => x.Term.Code)));
            }
        }

        // TODO: CalcTermCode goes somewhere else.
        static int CalcTermCode(string term)
        {
            int code = 0;

            var parts = term.Split(' ');
            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out code))
                {
                    code *= 10;

                    switch (parts[1].ToLower())
                    {
                        case "winter":
                            code += 1;
                            break;
                        case "spring":
                            code += 2;
                            break;
                        case "fall":
                            code += 3;
                            break;
                        default:
                            break;
                    }
                }
            }

            return code;
        }
    }
}
