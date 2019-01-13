using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BennyAdvisor.Models;

namespace UnitTests
{
    public class GroupBase
    {
        protected readonly GroupCollectionModel<string> Group;

        public GroupBase()
        {
            Group = new GroupCollectionModel<string>()
            {
                LastModified = DateTime.Parse("2018-01-01T00:00:00"),
                Groups = new[]
                {
                    new GroupModel<string>()
                    {
                        Id = "1",
                        Title = "Group 1",
                        Members = new[] { "2", "4", "6", "8" }
                    },
                    new GroupModel<string>()
                    {
                        Id = "2",
                        Title = "Group 2",
                        Members = new[] { "21", "23", "25", "27", "29" }
                    }
                }
            };
        }

        protected void VerifyEqual(GroupCollectionModel<string> g1, GroupCollectionModel<string> g2)
        {
            Assert.Equal(g1.LastModified, g2.LastModified);
            // Confirm that we actually have items.
            Assert.True(g1.Groups.Count() > 0);
            Assert.Equal(g1.Groups.Count(), g2.Groups.Count());
            Assert.Equal(g1.Groups.Count(), g2.Groups.Count());

            var g1List = g1.Groups.ToList();
            var g2List = g2.Groups.ToList();

            for (int i = 0; i < g1List.Count; i++)
                VerifyEqual(g1List[i], g2List[i]);
        }

        protected void VerifyEqual(GroupModel<string> g1, GroupModel<string> g2)
        {
            Assert.Equal(g1.Id, g2.Id);
            Assert.Equal(g1.Title, g2.Title);
            // Confirm that we actually have items.
            Assert.True(g1.Members.Count() > 0);
            Assert.Equal(g1.Members.Count(), g2.Members.Count());
            Assert.Equal(g1.Members, g2.Members);
        }
    }
}
