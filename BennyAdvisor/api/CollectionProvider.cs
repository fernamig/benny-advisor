using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class CollectionProvider<T>
    {
        protected GroupProvider<T> Provider { get; private set; }

        public CollectionProvider(string root)
        {
            Provider = new GroupProvider<T>(root);
        }

        public IEnumerable<T> Get(string id)
        {
            var group = Provider.TryGet(id);
            if ((group != null) && (group.Groups.Count() > 0))
                return group.Groups.First().Members;
            return Enumerable.Empty<T>();
        }

        public void Set(string id, IEnumerable<T> items)
        {
            var group = new GroupCollectionModel<T>()
            {
                LastModified = DateTime.UtcNow,
                Groups = new[]
                {
                    new GroupModel<T>()
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
