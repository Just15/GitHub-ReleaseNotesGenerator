using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class SearchIssueSectionRequestBuilder
    {
        public static SearchIssueSectionRequest CreateUnlabeledRequest(string repositoryFullName, string milestoneTitle)
        {
            var searchIssueSectionRequest = new SearchIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji("unlabeled"),
                Title = "Unlabeled",
                SearchIssuesRequest = new SearchIssuesRequest
                {
                    Repos = new RepositoryCollection
                    {
                        repositoryFullName
                    },
                    Milestone = milestoneTitle,
                    No = IssueNoMetadataQualifier.Label
                }
            };
            return searchIssueSectionRequest;
        }
    }
}
