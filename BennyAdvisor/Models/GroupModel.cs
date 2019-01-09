using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class GroupCollectionModel
    {
        public DateTime LastModified { get; set; }
        public IEnumerable<GroupModel> Groups { get; set; }
    }

    public class GroupModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Members { get; set; }
    }
}
