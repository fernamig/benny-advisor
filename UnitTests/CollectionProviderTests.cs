using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BennyAdvisor.api;
using BennyAdvisor.Models;

namespace UnitTests
{
    public class CollectionProviderTests
    {
        readonly CollectionProvider Provider = new CollectionProvider("test.collection");

        [Fact]
        public void VerifyNonExisting()
        {
            Assert.Equal(Enumerable.Empty<string>(), Provider.Get("dummy"));
        }

        [Fact]
        public void Verify()
        {
            var id = Guid.NewGuid().ToString();
            var testItems = new[] { "A", "B", "C", "D", "E" };

            Assert.Equal(Enumerable.Empty<string>(), Provider.Get(id));
            Provider.Set(id, testItems);
            Assert.Equal(testItems, Provider.Get(id));
            Provider.Delete(id);
            Assert.Equal(Enumerable.Empty<string>(), Provider.Get(id));
        }
    }
}
