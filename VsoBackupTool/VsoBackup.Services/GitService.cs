using System.Web;
using LibGit2Sharp;
using VsoBackup.Configuration;
using VsoBackup.Logging;
using VsoBackup.Models;

namespace VsoBackup.Services
{
    public class GitService : IGitService
    {
        private readonly IAllConfiguration _allConfiguration;
        private readonly ILogger _logger;
        private Credentials _credentials;
        public GitService(IAllConfiguration allConfiguration,ILogger logger)
        {
            _allConfiguration = allConfiguration;
            _logger = logger;
        }

        private Credentials CredentialsProvider(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            _credentials = new UsernamePasswordCredentials
            {
                Username = _allConfiguration.VsoConfiguration.ApiUsername,
                Password = _allConfiguration.VsoConfiguration.ApiPassword
            };
            return _credentials;
        }

        public void Clone(Value value, string path)
        {
            _logger.WriteLog("Cloning repository '{0}'", value.name);
            var pathlocal = Repository.Clone(HttpUtility.UrlPathEncode(value.remoteUrl), path, new CloneOptions()
             {
                 CredentialsProvider = CredentialsProvider
             });
            var repo = new Repository(pathlocal);
            foreach (var branch in repo.Branches)
            {
                string localName = branch.Name;
                if (localName.Contains("origin/") && !localName.Contains("master"))
                {
                    try
                    {
                        localName = localName.Remove(0, 7);                       
                        Branch newLocalBranch = repo.CreateBranch(localName, branch.Tip);
                        // Make the local branch track the upstream one
                        repo.Branches.Update(newLocalBranch,
                             b => b.TrackedBranch = branch.CanonicalName);
                        Branch trackingBranch = repo.Branches[localName];
                        repo.Checkout(trackingBranch);
                    }
                    catch(System.Exception ex)
                    {
                        //Gets exception of some repos when trying to create an existing branch..
                        //NameConflictException
                    }
                }
            }

        }
    }
}
