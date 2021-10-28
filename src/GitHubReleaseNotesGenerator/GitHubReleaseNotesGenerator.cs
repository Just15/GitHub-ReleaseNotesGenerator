using Octokit;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

    public class GitHubReleaseNotesGenerator
    {
        private string repositoryOwner;
        private string repositoryName;
        private GitHubClient gitHubClient;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials)
        {
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;

            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };
        }

        public async Task GenerateReleaseNotes()
        {
            var milestones = await gitHubClient.Issue.Milestone.GetAllForRepository(repositoryOwner, repositoryName);

            var allIssues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName);
        }
    }
}
