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
        public void GetNonExistingMustReturnNull()
        {
            Assert.Null(Provider.Get("dummy"));
        }

        [Fact]
        public void DeleteNonExisting()
        {
            Assert.Null(Provider.Get("dummy"));
            Provider.Delete("dummy");
            Assert.Null(Provider.Get("dummy"));
        }

        [Fact]
        public void VerifyGetSetDelete()
        {
            const string ownerId = "test";

            Assert.Null(Provider.Get(ownerId));
            Provider.Set(ownerId, Group1);
            VerifyEqual(Group1, Provider.Get(ownerId));

            Provider.Delete(ownerId);
            Assert.Null(Provider.Get(ownerId));
        }
    }
}
