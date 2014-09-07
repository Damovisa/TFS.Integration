using System.Collections.Generic;

namespace AlmDashboard.Models
{
    public class GitRepositoryCollection
    {
        public int Count { get; set; }
        public IEnumerable<GitRepository> Value { get; set; }
    }

    public class GitRepository
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GitProject Project { get; set; }
        public string DefaultBranch { get; set; }
        public string RemoteUrl { get; set; }
    }

    public class GitProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
