using System.Configuration;
using Nancy.ModelBinding;
using WorkItemVoter.VSOHelpers;

namespace WorkItemVoter
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        private VsoHelper _helper;
        public IndexModule()
        {
            var instance = ConfigurationManager.AppSettings["VSOInstance"];
            var user = ConfigurationManager.AppSettings["VSOUsername"];
            var pass = ConfigurationManager.AppSettings["VSOPassword"];
            _helper = new VsoHelper(instance, user, pass);

            // start page
            Get["/"] = parameters =>
            {
                var workItems = _helper.GetTwoPendingWorkItems();
                return View["Index", workItems];
            };

            // voting
            Post["/vote"] = parameters =>
            {
                var details = this.Bind<Vote>();
                var pushed = _helper.VoteForWorkItemAndNotifyOfApproval(details.id, details.rev);
                return pushed;
            };
        }
    }

    public class Vote
    {
        public int id { get; set; }
        public int rev { get; set; }
    }
}