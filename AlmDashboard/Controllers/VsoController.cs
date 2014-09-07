using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using AlmDashboard.Models;
using AlmDashboard.VSOHelpers;

namespace AlmDashboard.Controllers
{
    public class VsoController : ApiController
    {
        private readonly VsoHelper _helper;

        public VsoController()
        {
            var instance = ConfigurationManager.AppSettings["VSOInstance"];
            var user = ConfigurationManager.AppSettings["VSOUsername"];
            var pass = ConfigurationManager.AppSettings["VSOPassword"];
            _helper = new VsoHelper(instance, user, pass);
        }

        [HttpGet]
        [Route("api/Projects")]
        public IEnumerable<TfsProject> GetProjects()
        {
            return _helper.GetProjects();
        }

        [HttpGet]
        [Route("api/Builds")]
        public IEnumerable<TfsBuild> GetBuilds()
        {
            return _helper.GetBuilds();
        }
        
        [HttpGet]
        [Route("api/Changesets")]
        public IEnumerable<TfvcChangeset> GetChangesets()
        {
            return _helper.GetChangesets();
        }

        [HttpGet]
        [Route("api/GitRepos")]
        public IEnumerable<GitRepository> GetGitRepos()
        {
            return _helper.GetGitRepositories();
        } 
    }
}
