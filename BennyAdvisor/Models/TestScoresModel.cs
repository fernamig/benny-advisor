using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class TestScoresModel
    {
        public DateTime FileDate { get; set; }
        public IEnumerable<TestScoreModel> Scores { get; set; }
    }
}
