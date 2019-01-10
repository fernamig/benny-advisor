using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class CollectionProvider
    {
        protected GroupProvider Provider { get; private set; }

        public CollectionProvider(string root)
        {
            Provider = new GroupProvider(root);
        }

        public IEnumerable<string> Get(string id)
        {
            var group = Provider.TryGet(id);
            if ((group != null) && (group.Groups.Count() > 0))
                return group.Groups.First().Members;
            return Enumerable.Empty<string>();
        }

        public void Set(string id, IEnumerable<string> items)
        {
            var group = new GroupCollectionModel()
            {
                LastModified = DateTime.UtcNow,
                Groups = new[]
                {
                    new GroupModel()
                    {
                        Id = "",
                        Title = "",
                        Members = items
                    }
                }
            };
            Provider.Set(id, group);
        }

        public void Delete(string id)
        {
            Provider.Delete(id);
        }
    }
}
