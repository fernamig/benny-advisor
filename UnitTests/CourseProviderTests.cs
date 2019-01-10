using System;
using System.Linq;
using Xunit;
using BennyAdvisor.api;
using BennyAdvisor.Models;

namespace UnitTests
{
    public class CourseProviderTests
    {
        readonly CourseProvider Provider = new CourseProvider();

        [Fact]
        public void VerifyNonExisting()
        {
            Assert.Throws<AggregateException>(() => Provider.Get("dummy"));
            Assert.Throws<AggregateException>(() => Provider.GetAsync("dummy").Result);
            Assert.Null(Provider.TryGet("dummy"));
            Assert.Null(Provider.TryGetAsync("dummy").Result);
        }

        [Fact]
        public void VerifyGetTitles()
        {
            var titles = Provider.GetAllTitles();
            Assert.NotNull(titles);
            Assert.True(titles.Count() > 0);

            var c = titles.Where(x => x.Code == "COMM111").FirstOrDefault();
            Assert.NotNull(c);
            Assert.False(string.IsNullOrWhiteSpace(c.Title));

            c = titles.Where(x => x.Code == "CS584").FirstOrDefault();
            Assert.NotNull(c);
            Assert.False(string.IsNullOrWhiteSpace(c.Title));
        }
    }
}
