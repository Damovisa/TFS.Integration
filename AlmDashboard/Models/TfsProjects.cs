using System.Collections.Generic;

namespace AlmDashboard.Models
{
    public class TfsProjectCollection
    {
        public IEnumerable<TfsProject> Value { get; set; }
    }

    public class TfsProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public TfsCollection Collection { get; set; }
        public TfsTeam DefaultTeam { get; set; }
    }

    public class TfsTeam
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class TfsCollection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string CollectionUrl { get; set; }
    }
}