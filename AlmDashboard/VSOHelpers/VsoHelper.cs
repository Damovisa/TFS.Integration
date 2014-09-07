using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using AlmDashboard.Models;

namespace AlmDashboard.VSOHelpers
{
    public class VsoHelper
    {
        private readonly string _account;
        private readonly string _username;
        private readonly string _password;

        public VsoHelper(string account, string username, string password)
        {
            _account = account;
            _username = username;
            _password = password;
        }

        public IEnumerable<TfsProject> GetProjects()
        {
            return GetVsoRequest<TfsProjectCollection>("projects").Value;
        }

        public IEnumerable<TfvcChangeset> GetChangesets()
        {
            return GetVsoRequest<TfvcChangesetCollection>("tfvc/changesets?$top=10&$orderby=createdDate desc").Value;
        }

        public IEnumerable<TfsBuild> GetBuilds()
        {
            return GetVsoRequest<TfsBuildCollection>("build/builds?$top=10").Value;
        }

        public IEnumerable<GitRepository> GetGitRepositories()
        {
            return GetVsoRequest<GitRepositoryCollection>("git/repositories").Value;
        }

        private TResult GetVsoRequest<TResult>(string uriResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", _username, _password))));

                using (HttpResponseMessage response = client.GetAsync(
                            "https://" + _account + ".visualstudio.com/DefaultCollection/_apis/"+uriResource).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsAsync<TResult>().Result;
                }
            }
        }

    }

}