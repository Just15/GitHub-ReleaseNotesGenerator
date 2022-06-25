using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class GitHubReleaseNotesGenerator
    {
        private readonly GitHubClient gitHubClient;
        private readonly Repository repository;
        private readonly GitHubSearchIssuesService gitHubSearchIssuesService;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName)
        {
            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName));
        }

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials) : this(repositoryOwner, repositoryName)
        {
            gitHubClient.Credentials = credentials;
            repository = gitHubClient.Repository.Get(repositoryOwner, repositoryName).Result;
            gitHubSearchIssuesService = new GitHubSearchIssuesService(gitHubClient, repository);
        }

        public async Task<string> Generate(string milestoneTitle, Changelog changelog)
        {
            var milestone = gitHubClient.Issue.Milestone.GetAllForRepository(repository.Id).Result.SingleOrDefault(m => m.Title == milestoneTitle);
            if (milestone == null)
            {
                throw new ArgumentException($"A milestone with name '{milestoneTitle}' doesn't exist.");
            }

            var searchIssuesResults = await gitHubSearchIssuesService.Search(milestoneTitle, changelog);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(MarkdownGenerator.GenerateIssueText(searchIssuesResults));
            if (changelog.IncludeContributors)
            {
                stringBuilder.Append(MarkdownGenerator.GenerateContributorsText(searchIssuesResults));
            }
            return stringBuilder.ToString();
        }
    }
}
