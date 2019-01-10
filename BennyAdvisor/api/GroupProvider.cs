using System;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class GroupProvider
    {
        public AwsBucket Bucket { get; private set; }

        public GroupProvider(string root)
        {
            Bucket = new AwsBucket(root);
        }

        public GroupCollectionModel Get(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            return Bucket.ReadObject<GroupCollectionModel>(keyName);
        }
        public GroupCollectionModel TryGet(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            return Bucket.TryReadObject<GroupCollectionModel>(keyName);
        }

        public void Set(string ownerId, GroupCollectionModel groups)
        {
            var keyName = $"{ownerId}.json";

            // Set the last update time stamp and the ID of each group.
            groups.LastModified = DateTime.UtcNow;
            foreach (var g in groups.Groups)
                g.Id = Guid.NewGuid().ToString();
            Bucket.WriteObject(keyName, groups);
        }

        public void Delete(string ownerId)
        {
            var keyName = $"{ownerId}.json";
            Bucket.DeleteFile(keyName);
        }
    }
}
