using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    internal class GitHubSearchIssuesService
    {
        private readonly GitHubClient gitHubClient;
        private readonly Repository repository;

        public GitHubSearchIssuesService(GitHubClient gitHubClient, Repository repository)
        {
            this.gitHubClient = gitHubClient;
            this.repository = repository;
        }

        internal async Task<List<(string, SearchIssuesResult)>> Search(string milestoneTitle, Changelog changelog)
        {
            var searchIssuesResults = new List<(string, SearchIssuesResult)>();
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

                searchIssuesResults.Add((category.Title, searchIssuesResult));
            }

            return searchIssuesResults;
        }
    }
}
