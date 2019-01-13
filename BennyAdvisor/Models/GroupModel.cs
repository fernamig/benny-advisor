using System;
using System.Collections.Generic;

namespace BennyAdvisor.Models
{
    public class GroupCollectionModel<T>
    {
        public DateTime LastModified { get; set; }
        public IEnumerable<GroupModel<T>> Groups { get; set; }
    }

    public class GroupModel<T>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<T> Members { get; set; }
    }
}
