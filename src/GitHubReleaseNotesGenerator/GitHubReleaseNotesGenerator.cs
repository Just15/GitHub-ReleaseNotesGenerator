using Octokit;
using System;
using System.Net.Http;
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
        public string PostGenerateNotesUri
        {
            get { return $"{RepositoryOwner}/{RepositoryName}/releases/generate-notes"; }
        }

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
            var repository = await gitHubClient.Repository.Get("Just15", "GitVersion-PdfSharpWrapper");

            // Post - /repos/{owner}/{repo}/releases/generate-notes
            var uriString = $"https://api.github.com/repos/{PostGenerateNotesUri}";
            IApiResponse<HttpResponseMessage> apiResponse = await gitHubClient.Connection.Post<HttpResponseMessage>(new Uri(uriString),
                new GenerateNotes("1.0.5") , "application/json", "application/json");
        }
    }
}
