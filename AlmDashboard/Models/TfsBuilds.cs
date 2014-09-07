using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmDashboard.Models
{
    public class TfsBuildCollection
    {
        public int Count { get; set; }
        public IEnumerable<TfsBuild> Value { get; set; }
    }

    public class TfsBuild
    {
        public string Uri { get; set; }
        public int Id { get; set; }
        public BuildProject Project { get; set; }
        public string BuildNumber { get; set; }
        public string Url { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string DropLocation { get; set; }
        public string SourceGetVersion { get; set; }
        public Author LastChangedBy { get; set; }
        public bool KeepForever { get; set; }
        public bool HasDiagnostics { get; set; }
        public BuildDefinition Definition { get; set; }
        public BuildQueue Queue { get; set; }
        public IEnumerable<BuildRequest> Requests { get; set; }
    }

    public class BuildRequest
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public AuthorWithUrl RequestedFor { get; set; }
    }

    public class BuildQueue
    {
        public int Id { get; set; }
        public string QueueType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BuildDefinition
    {
        public int Id { get; set; }
        public string DefinitionType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BuildProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}