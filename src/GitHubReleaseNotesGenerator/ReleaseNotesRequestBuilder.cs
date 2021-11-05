using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class ReleaseNotesRequestBuilder
    {
        public static ReleaseNotesRequest CreateDefaultReleaseNotesRequest(Repository repository, Milestone milestone)
        {
            // Enhancements
            var enhancementsSectionRequest = new RepositoryIssueSectionRequest
            {
                DefultEmoji = ":star:",
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestone.Number.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            enhancementsSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");

            // Bugs
            var bugsSectionRequest = new RepositoryIssueSectionRequest
            {
                DefultEmoji = ":beetle:",
                Title = "Bugs",
                RepositoryIssueRequest = new RepositoryIssueRequest
                {
                    Milestone = milestone.Number.ToString(),
                    State = ItemStateFilter.Closed
                }
            };
            bugsSectionRequest.RepositoryIssueRequest.Labels.Add("bug");

            // Unlabeled
            var unlabeledSectionRequest = new SearchIssueSectionRequest
            {
                DefultEmoji = ":pushpin:",
                Title = "Unlabeled",
                SearchIssuesRequest = new SearchIssuesRequest
                {
                    Repos = new RepositoryCollection
                    {
                        repository.FullName
                    },
                    Milestone = milestone.Title,
                    No = IssueNoMetadataQualifier.Label
                }
            };

            // Request
            return new ReleaseNotesRequest
            {
                Milestone = milestone.Title,
                RepositoryIssueSections = new List<RepositoryIssueSectionRequest>
                {
                    enhancementsSectionRequest,
                    bugsSectionRequest
                },
                SearchIssueSections = new List<SearchIssueSectionRequest>
                {
                    unlabeledSectionRequest
                }
            };
        }

        public static async Task<ReleaseNotesRequest> CreateReleaseNotesForAllLabels(GitHubClient gitHubClient, Repository repository, Milestone milestone)
        {
            // Repository Issue Sections
            var repositoryIssueSections = new List<RepositoryIssueSectionRequest>();
            foreach (var label in (await gitHubClient.Issue.Labels.GetAllForMilestone(repository.Id, milestone.Number)).Select(label => label.Name))
            {
                var section = new RepositoryIssueSectionRequest
                {
                    DefultEmoji = EmojiHelper.TryGetEmoji(label),
                    Title = label,
                    RepositoryIssueRequest = new RepositoryIssueRequest
                    {
                        Milestone = milestone.Number.ToString(),
                        State = ItemStateFilter.Closed
                    }
                };
                section.RepositoryIssueRequest.Labels.Add(label);

                repositoryIssueSections.Add(section);
            }

            // Search Issue Section
            var searchIssueSections = new List<SearchIssueSectionRequest>
            {
                new SearchIssueSectionRequest
                {
                    DefultEmoji = ":pushpin:",
                    Title = "Unlabeled",
                    SearchIssuesRequest = new SearchIssuesRequest
                    {
                        Repos = new RepositoryCollection
                        {
                            repository.FullName
                        },
                        Milestone = milestone.Title,
                        No = IssueNoMetadataQualifier.Label
                    }
                }
            };

            // Request
            return new ReleaseNotesRequest
            {
                Milestone = milestone.Title,
                RepositoryIssueSections = repositoryIssueSections,
                SearchIssueSections = searchIssueSections
            };
        }
    }
}
