using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LightweightCommentChecker
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        private VsoHelper _vsoHelper;

        public IndexModule()
        {
            _vsoHelper = new VsoHelper();
            Get["/"] = parameters =>
            {
                return View["index"];
            };

            Get["/GetChangeSets"] = _ =>
            {
                var url = "http://damo-tfs:8080/tfs/defaultcollection/_apis/tfvc/changesets?fromDate=" +
                          DateTime.Now.AddDays(-1).ToString("yyyy-M-d");
                return _vsoHelper.GetVsoRequest(url);
            };
        }
    }

    public class VsoHelper
    {
        public string GetVsoRequest(string url)
        {
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}