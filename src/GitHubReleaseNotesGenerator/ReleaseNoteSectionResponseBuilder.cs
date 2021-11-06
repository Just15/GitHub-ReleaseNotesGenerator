using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class ReleaseNoteSectionResponseBuilder
    {
        public static async Task<ReleaseNoteSectionResponse> Create(GitHubClient gitHubClient, Repository repository, RepositoryIssueSectionRequest repositoryIssueSectionRequest)
        {
            var issues = await gitHubClient.Issue.GetAllForRepository(repository.Id, repositoryIssueSectionRequest.RepositoryIssueRequest);

            return new ReleaseNoteSectionResponse
            {
                Emoji = repositoryIssueSectionRequest.Emoji,
                DefultEmoji = repositoryIssueSectionRequest.DefultEmoji,
                Title = repositoryIssueSectionRequest.Title,
                Issues = issues
            };
        }

        public static async Task<List<ReleaseNoteSectionResponse>> Create(GitHubClient gitHubClient, Repository repository, List<RepositoryIssueSectionRequest> repositoryIssueSectionRequests)
        {
            var response = new List<ReleaseNoteSectionResponse>();

            foreach (var section in repositoryIssueSectionRequests)
            {
                response.Add(await Create(gitHubClient, repository, section));
            }

            return response;
        }

        public static async Task<ReleaseNoteSectionResponse> Create(GitHubClient gitHubClient, SearchIssueSectionRequest searchIssueSectionRequest)
        {
            var searchIssueResult = await gitHubClient.Search.SearchIssues(searchIssueSectionRequest.SearchIssuesRequest);

            return new ReleaseNoteSectionResponse
            {
                Emoji = searchIssueSectionRequest.Emoji,
                DefultEmoji = searchIssueSectionRequest.DefultEmoji,
                Title = searchIssueSectionRequest.Title,
                Issues = searchIssueResult.Items
            };
        }

        public static async Task<List<ReleaseNoteSectionResponse>> Create(GitHubClient gitHubClient, List<SearchIssueSectionRequest> searchIssueSectionRequests)
        {
            var response = new List<ReleaseNoteSectionResponse>();

            foreach (var section in searchIssueSectionRequests)
            {
                response.Add(await Create(gitHubClient, section));
            }

            return response;
        }
    }
}
