using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using WorkItemVoter.Models;

namespace WorkItemVoter.VSOHelpers
{
    public class VsoHelper
    {
        private readonly string _account;
        private readonly string _username;
        private readonly string _password;
        private readonly int _voteCount;

        public VsoHelper(string account, string username, string password)
        {
            _account = account;
            _username = username;
            _password = password;
            _voteCount = 2;
        }

        public WorkItems GetTwoPendingWorkItems()
        {
            // 1. run query to get WIs that are "New"
            var query = new TfsQuery
            {
                wiql =
                    "SELECT [System.Id],[System.Title] FROM WorkItems WHERE [System.TeamProject]='IntegrationDemo' " +
                    "AND [System.WorkItemType]='Product Backlog Item' AND [System.State]='New'"
            };
            var results = PostVsoQuery("wit/queryresults?@project=IntegrationDemo", query);


            // 2. if at least 2, get the work item details
            if (results.results != null && results.results.Count() >= 2)
            {
                var ids = results.results.Take(2).Select(i => i.sourceId);
                var workitemstring = 
                    GetVsoRequest(string.Format("wit/workitems?ids={0}&fields=system.id,system.rev,system.title",
                    string.Join(",", ids)));
                dynamic wiresult = JObject.Parse(workitemstring);
                var workitems = new WorkItems()
                {
                    FirstId = wiresult.value[0].fields[0].value,
                    FirstRev = wiresult.value[0].fields[1].value,
                    FirstTitle = wiresult.value[0].fields[2].value,
                    SecondId = wiresult.value[1].fields[0].value,
                    SecondRev = wiresult.value[1].fields[1].value,
                    SecondTitle = wiresult.value[1].fields[2].value
                };
                return workitems;
            }
            return null;
        }

        public VoteResult VoteForWorkItemAndNotifyOfApproval(int id, int rev)
        {
            // 1. Update the counter for this work item
            var curCount = (int)(System.Web.HttpContext.Current.Cache["wi-" + id] ?? 0);
            System.Web.HttpContext.Current.Cache["wi-" + id] = curCount + 1;
            if (curCount >= _voteCount-1)
            {
                // move this to Approved
                var content = "{\"id\":"+id+",\"rev\":"+rev+",\"fields\":[{\"field\":{\"refName\":\"System.State\"},\"value\":\"Approved\"},{\"field\":{\"refName\":\"System.Reason\"},\"value\":\"Approved by the Product Owner\"}]}";
                try
                {
                    PatchVsoRequest(string.Format("wit/workitems/{0}", id), content);
                    return new VoteResult {VoteSucceeded = true, ItemApproved = true};
                }
                catch (Exception)
                {
                    return new VoteResult { VoteSucceeded = false, ItemApproved = false, ErrorMessage = "The work item has been approved since you loaded the page."};
                }
            }
            return new VoteResult() {VoteSucceeded = true, ItemApproved = false};
        }

        private string GetVsoRequest(string uriResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", _username, _password))));


                using (HttpResponseMessage response = client.GetAsync(
                            "https://" + _account + ".visualstudio.com/DefaultCollection/_apis/"+uriResource)
                            .Result)
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        private QueryResult PostVsoQuery(string uriResource, TfsQuery tfsQuery)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", _username, _password))));


                using (HttpResponseMessage response = client.PostAsJsonAsync(
                            "https://" + _account + ".visualstudio.com/DefaultCollection/_apis/" + uriResource, tfsQuery)
                            .Result)
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsAsync<QueryResult>().Result;
                }
            }
        }

        private bool PatchVsoRequest(string uriResource, string strContent)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", _username, _password))));


                var content = new StringContent(strContent);
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var httpMessage = new HttpRequestMessage(
                    new HttpMethod("PATCH"),
                    "https://" + _account + ".visualstudio.com/DefaultCollection/_apis/" + uriResource)
                {
                    Content = content,
                };
                using (HttpResponseMessage response = client.SendAsync(httpMessage).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return true;
                }
            }
        }
        
        public class TfsQuery
        {
            public string wiql { get; set; }
        }

        public class QueryResult
        {
            public string asOf { get; set; }
            public WorkItemId[] results { get; set; }
        }

        public class WorkItemId
        {
            public int sourceId { get; set; }
        }


    }

    public class VoteResult
    {
        public bool VoteSucceeded { get; set; }
        public bool ItemApproved { get; set; }
        public string ErrorMessage { get; set; }
    }
}