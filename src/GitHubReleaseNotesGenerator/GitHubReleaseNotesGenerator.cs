using Octokit;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

    // https://docs.github.com/en/rest/reference/repos#generate-release-notes-content-for-a-release

    // Get repository ID, maybe
    // https://codesnippet.io/github-api-tutorial/

    public class GitHubReleaseNotesGenerator
    {
        public string RepositoryOwner { get; private set; }
        public string RepositoryName { get; private set; }

        private GitHubClient gitHubClient;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials)
        {
            RepositoryOwner = repositoryOwner;
            RepositoryName = repositoryName;

            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };
        }

        public async Task DoStuff()
        {
            Octokit.Repository repository = await gitHubClient.Repository.Get("Just15", "PdfSharpWrapper");
        }
    }
}
