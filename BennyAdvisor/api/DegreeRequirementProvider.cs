using System;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class DegreeRequirementProvider
    {
        private AwsBucket Bucket { get; set; }

        public DegreeRequirementProvider()
        {
            Bucket = new AwsBucket("degree-requirement");
        }

        public DegreeRequirementSetModel[] Get(string id)
        {
            return Bucket.ReadArray<DegreeRequirementSetModel>($"{id}.json");
        }

    }
}
