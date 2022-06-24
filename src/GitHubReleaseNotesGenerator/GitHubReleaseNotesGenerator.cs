using System;
using System.Collections.Generic;
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

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName)
        {
            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName));
        }

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials) : this(repositoryOwner, repositoryName)
        {
            gitHubClient.Credentials = credentials;
            repository = gitHubClient.Repository.Get(repositoryOwner, repositoryName).Result;
        }

        public async Task<string> Generate(string milestoneTitle, Changelog changelog)
        {
            var milestone = gitHubClient.Issue.Milestone.GetAllForRepository(repository.Id).Result.SingleOrDefault(m => m.Title == milestoneTitle);
            if (milestone == null)
            {
                throw new ArgumentException($"A milestone with name '{milestoneTitle}' doesn't exist.");
            }

            var stringBuilder = new StringBuilder();
            foreach (var category in changelog.Categories)
            {
                var searchIssuesRequest = new SearchIssuesRequest
                {
                    Repos = new RepositoryCollection { repository.FullName },
                    Milestone = milestoneTitle,
                    Type = IssueTypeQualifier.Issue,
                    State = ItemState.Closed,
                };

                if (category.Labels != null && category.Labels.All(label => !string.IsNullOrEmpty(label)))
                {
                    searchIssuesRequest.Labels = category.Labels;
                }
                else
                {
                    searchIssuesRequest.No = IssueNoMetadataQualifier.Label;
                }

                var searchIssuesResult = await gitHubClient.Search.SearchIssues(searchIssuesRequest);

                MarkdownGenerator.GenerateIssueText(stringBuilder, category.Title, searchIssuesResult);
            }

            return stringBuilder.ToString();
        }
    }
}
