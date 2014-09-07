using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace ClientObjectModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var tfs = new TfsTeamProjectCollection(
                new Uri("http://localhost:8080/tfs/DefaultCollection"),
                new TfsClientCredentials());

            var vcs = tfs.GetService<VersionControlServer>();

            // get the latest changeset
            var latestChangesetId = vcs.GetLatestChangesetId();
            var latestChangeSet = vcs.GetChangeset(latestChangesetId);

            Console.WriteLine("Latest Changeset: {0}", latestChangesetId);
            Console.WriteLine("Committer: {0}", latestChangeSet.CommitterDisplayName);
            Console.WriteLine("Comment: {0}", latestChangeSet.Comment);

            // update the comment to add "Performance Improvement"
            latestChangeSet.Comment += (Environment.NewLine + "Performance Improvement");
            latestChangeSet.Update();

            // check it again
            latestChangeSet = vcs.GetChangeset(latestChangesetId);
            Console.WriteLine("Latest Changeset: {0}", latestChangesetId);
            Console.WriteLine("Committer: {0}", latestChangeSet.CommitterDisplayName);
            Console.WriteLine("Comment: {0}", latestChangeSet.Comment);

            // undo the change
            latestChangeSet.Comment = latestChangeSet.Comment.Replace(Environment.NewLine + "Performance Improvement","");
            latestChangeSet.Update();

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
