using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class TestScoresProvider
    {
        readonly ObjectProvider<TestScoresModel> Provider;

        public TestScoresProvider()
        {
            Provider = new ObjectProvider<TestScoresModel>("test-scores");
        }

        public TestScoresModel Get(string id)
        {
            return Provider.TryGet(id) ??
                new TestScoresModel() { FileDate = DateTime.UtcNow, Scores = Enumerable.Empty<TestScoreModel>() };
        }

        public void Add(string id, TestScoreModel score)
        {
            var scores = Get(id);
            scores.Scores = scores.Scores.Concat(new[] { score });
            scores.FileDate = DateTime.UtcNow;
            Provider.Set(id, scores);
        }
    }
}
