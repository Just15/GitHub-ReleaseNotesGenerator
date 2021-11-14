using System.Collections.Generic;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class RepositoryIssueSectionRequestBuilder
    {
        public static RepositoryIssueSectionRequest CreateRequest(int milestoneNumber, SectionRequest sectionRequest)
        {
            var repositoryIssueSectionRequest = new RepositoryIssueSectionRequest
            {
                Emoji = EmojiHelper.TryGetEmoji(sectionRequest.Emoji),
                Title = sectionRequest.Title,
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestoneNumber.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            repositoryIssueSectionRequest.RepositoryIssueRequest.Labels.Add(sectionRequest.Label);
            return repositoryIssueSectionRequest;
        }

        public static List<RepositoryIssueSectionRequest> CreateRequests(int milestoneNumber, List<SectionRequest> sectionRequests)
        {
            var requests = new List<RepositoryIssueSectionRequest>();
            foreach (var sectionRequest in sectionRequests)
            {
                requests.Add(CreateRequest(milestoneNumber, sectionRequest));
            }

            return requests;
        }

        public static RepositoryIssueSectionRequest CreateEnhancementRequest(int milestoneNumber)
        {
            return CreateRequest(milestoneNumber, SectionRequestBuilder.CreateEnhancement());
        }

        public static RepositoryIssueSectionRequest CreateBugRequest(int milestoneNumber)
        {
            return CreateRequest(milestoneNumber, SectionRequestBuilder.CreateBug());
        }

        public static RepositoryIssueSectionRequest CreateBuildRequest(int milestoneNumber)
        {
            return CreateRequest(milestoneNumber, SectionRequestBuilder.CreateBuild());
        }

        public static RepositoryIssueSectionRequest CreateDocumentationRequest(int milestoneNumber)
        {
            return CreateRequest(milestoneNumber, SectionRequestBuilder.CreateDocumentation());
        }
    }
}
