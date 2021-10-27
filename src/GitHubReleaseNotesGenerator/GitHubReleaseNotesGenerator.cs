using Octokit;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

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
    }
}
