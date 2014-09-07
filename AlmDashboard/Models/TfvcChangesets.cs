using System;
using System.Collections.Generic;

namespace AlmDashboard.Models
{
    public class TfvcChangesetCollection
    {
        public int Count { get; set; }
        public IEnumerable<TfvcChangeset> Value { get; set; }
    }

    public class TfvcChangeset
    {
        public int ChangesetId { get; set; }
        public Author Author { get; set; }
        public Author CheckedInBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }
    }
}