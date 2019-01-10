using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BennyAdvisor.api;

namespace UnitTests
{
    public class GroupProviderTests : GroupBase
    {
        readonly GroupProvider Provider = new GroupProvider("test.group");

        [Fact]
        public void GetNonExisting()
        {
            Assert.Null(Provider.TryGet("dummy"));
            Assert.Throws<AggregateException>(() => Provider.Get("dummy"));
        }

        [Fact]
        public void DeleteNonExisting()
        {
            Assert.Null(Provider.TryGet("dummy"));
            Provider.Delete("dummy");
            Assert.Null(Provider.TryGet("dummy"));
        }

        [Fact]
        public void VerifyGetSetDelete()
        {
            const string ownerId = "test";

            Assert.Null(Provider.TryGet(ownerId));
            Provider.Set(ownerId, Group);
            VerifyEqual(Group, Provider.Get(ownerId));

            Provider.Delete(ownerId);
            Assert.Null(Provider.TryGet(ownerId));
        }
    }
}
