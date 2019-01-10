using System;
using Xunit;
using BennyAdvisor.api;
using BennyAdvisor.Models;

namespace UnitTests
{
    public class StudentProviderTests
    {
        readonly StudentProvider Provider = new StudentProvider();
        readonly StudentModel Student;

        public StudentProviderTests()
        {
            Student = new StudentModel()
            {
                Id = "999999999",
                Level = "Undergraduate",
                ImagePath = "/some/path",
                FirstName = "FirstName",
                LastName = "LastName",
                DegreeName = "Some degree",
                DegreeCode = "ZZZ",
                Major = "My Major",
                Minor = "My Minor",
                OverallGpa = "4.0",
                Confidential = false,
            };
        }

        [Fact]
        public void VerifyNonExisting()
        {
            Assert.Throws<AggregateException>(() => Provider.Get("dummy"));
            Assert.Throws<AggregateException>(() => Provider.GetAsync("dummy").Result);
            Assert.Null(Provider.TryGet("dummy"));
            Assert.Null(Provider.TryGetAsync("dummy").Result);
        }

        [Fact]
        public void VerifySet()
        {
            var id = Guid.NewGuid().ToString();
            Assert.Null(Provider.TryGet(id));
            Provider.Set(id, Student);
            VerifyEqual(Student, Provider.Get(id));
            Provider.Delete(id);

            Assert.Null(Provider.TryGet(id));
            Provider.SetAsync(id, Student).Wait();
            VerifyEqual(Student, Provider.Get(id));
            Provider.Delete(id);
        }

        [Fact]
        public void VerifyDelete()
        {
            var id = Guid.NewGuid().ToString();
            Assert.Null(Provider.TryGet(id));
            Provider.Set(id, Student);
            VerifyEqual(Student, Provider.Get(id));
            Provider.Delete(id);
            Assert.Null(Provider.TryGet(id));

            Provider.Set(id, Student);
            VerifyEqual(Student, Provider.Get(id));
            Provider.DeleteAsync(id).Wait();
            Assert.Null(Provider.TryGet(id));
        }

        [Fact]
        public void VerifyKnownStudent()
        {
            var testStudent = new StudentModel()
            {
                Id = "000000001",
                Level = "Undergraduate",
                ImagePath = "/images/test.jpg",
                FirstName = "Tenesha",
                LastName = "Hazan",
                DegreeName = "Bachelor of Science",
                DegreeCode = "BS",
                Major = "Earth Sciences",
                Minor = "Forestry",
                OverallGpa = "3.4",
                Confidential = false,
            };
            VerifyEqual(testStudent, Provider.Get("test.1"));
        }

        void VerifyEqual(StudentModel s1, StudentModel s2)
        {
            Assert.Equal(s1.Id, s2.Id);
            Assert.Equal(s1.Level, s2.Level);
            Assert.Equal(s1.ImagePath, s2.ImagePath);
            Assert.Equal(s1.FirstName, s2.FirstName);
            Assert.Equal(s1.LastName, s2.LastName);
            Assert.Equal(s1.DegreeName, s2.DegreeName);
            Assert.Equal(s1.DegreeCode, s2.DegreeCode);
            Assert.Equal(s1.Major, s2.Major);
            Assert.Equal(s1.Minor, s2.Minor);
            Assert.Equal(s1.OverallGpa, s2.OverallGpa);
            Assert.Equal(s1.Confidential, s2.Confidential);
        }
    }
}
