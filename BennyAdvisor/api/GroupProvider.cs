using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class GroupProvider<T>
    {
        public AwsBucket Bucket { get; private set; }

        public GroupProvider(string root)
        {
            Bucket = new AwsBucket(root);
        }

        public GroupCollectionModel<T> Get(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            return Bucket.ReadObject<GroupCollectionModel<T>>(keyName);
        }
        public GroupCollectionModel<T> TryGet(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            return Bucket.TryReadObject<GroupCollectionModel<T>>(keyName);
        }

        public void Set(string ownerId, IEnumerable<GroupModel<T>> groups)
        {
            Set(ownerId, new GroupCollectionModel<T>()
            {
                LastModified = DateTime.UtcNow,
                Groups = groups
            });
        }
        public void Set(string ownerId, GroupCollectionModel<T> groups)
        {
            var keyName = $"{ownerId}.json";

            // Set the last update time stamp and the ID of each group.
            groups.LastModified = DateTime.UtcNow;
            foreach (var g in groups.Groups)
            {
                if (string.IsNullOrWhiteSpace(g.Id))
                    g.Id = Guid.NewGuid().ToString();
            }
            Bucket.WriteObject(keyName, groups);
        }

        public void Delete(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            Bucket.DeleteFile(keyName);
        }
    }
}
