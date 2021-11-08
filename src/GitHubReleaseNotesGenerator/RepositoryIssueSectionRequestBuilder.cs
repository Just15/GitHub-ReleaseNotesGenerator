using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class RepositoryIssueSectionRequestBuilder
    {
        public static RepositoryIssueSectionRequest CreateEnhancementRequest(int milestoneNumber)
        {
            var repositoryIssueSectionRequest = new RepositoryIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji("enhancement"),
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestoneNumber.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            repositoryIssueSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");
            return repositoryIssueSectionRequest;
        }

        public static RepositoryIssueSectionRequest CreateBugRequest(int milestoneNumber)
        {
            var repositoryIssueSectionRequest = new RepositoryIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji("bug"),
                Title = "Bugs",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestoneNumber.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            repositoryIssueSectionRequest.RepositoryIssueRequest.Labels.Add("bug");
            return repositoryIssueSectionRequest;
        }
    }
}
