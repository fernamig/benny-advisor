using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class AdvisorProvider : CollectionProvider<string>
    {
        static Dictionary<string, string> s_StudentAdvisorMap;

        static AdvisorProvider()
        {
            s_StudentAdvisorMap = new Dictionary<string, string>();

            var provider = new AdvisorProvider();
            Init(provider, "100000001");
        }

        public AdvisorProvider()
            : base("advisor")
        {
        }

        public string GetAdvisorForStudent(string studentId)
        {
            if (s_StudentAdvisorMap.ContainsKey(studentId))
                return s_StudentAdvisorMap[studentId];
            return null;
        }

        static void Init(AdvisorProvider provider, string advisorId)
        {
            foreach (var studentId in provider.Get(advisorId))
                s_StudentAdvisorMap[studentId] = advisorId;
        }
    }
}
