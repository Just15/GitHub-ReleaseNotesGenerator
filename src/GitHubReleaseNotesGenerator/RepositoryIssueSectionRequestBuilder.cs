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

        public static RepositoryIssueSectionRequest CreateBuildRequest(int milestoneNumber)
        {
            var repositoryIssueSectionRequest = new RepositoryIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji("build"),
                Title = "Build",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestoneNumber.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            repositoryIssueSectionRequest.RepositoryIssueRequest.Labels.Add("build");
            return repositoryIssueSectionRequest;
        }

        public static RepositoryIssueSectionRequest CreateDocumentationRequest(int milestoneNumber)
        {
            var repositoryIssueSectionRequest = new RepositoryIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji("documentation"),
                Title = "Documentation",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestoneNumber.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            repositoryIssueSectionRequest.RepositoryIssueRequest.Labels.Add("documentation");
            return repositoryIssueSectionRequest;
        }
    }
}
