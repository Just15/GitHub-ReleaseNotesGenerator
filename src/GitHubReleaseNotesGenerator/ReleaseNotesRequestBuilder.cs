using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class ReleaseNotesRequestBuilder
    {
        public static ReleaseNotesRequest CreateCustom(Repository repository, Milestone milestone, List<SectionRequest> sectionRequests)
        {
            return new ReleaseNotesRequest
            {
                Milestone = milestone.Title,
                RepositoryIssueSections = RepositoryIssueSectionRequestBuilder.CreateRequests(milestone.Number, sectionRequests),
                SearchIssueSections = new List<SearchIssueSectionRequest>
                {
                    SearchIssueSectionRequestBuilder.CreateUnlabeledRequest(repository.FullName, milestone.Title)
                }
            };
        }

        public static ReleaseNotesRequest CreateDefault(Repository repository, Milestone milestone)
        {
            return new ReleaseNotesRequest
            {
                Milestone = milestone.Title,
                RepositoryIssueSections = new List<RepositoryIssueSectionRequest>
                {
                    RepositoryIssueSectionRequestBuilder.CreateEnhancementRequest(milestone.Number),
                    RepositoryIssueSectionRequestBuilder.CreateBugRequest(milestone.Number)
                },
                SearchIssueSections = new List<SearchIssueSectionRequest>
                {
                    SearchIssueSectionRequestBuilder.CreateUnlabeledRequest(repository.FullName, milestone.Title)
                }
            };
        }

        public static async Task<ReleaseNotesRequest> CreateForAllLabels(GitHubClient gitHubClient, Repository repository, Milestone milestone)
        {
            // Repository Issue Sections
            var repositoryIssueSections = new List<RepositoryIssueSectionRequest>();
            foreach (var label in (await gitHubClient.Issue.Labels.GetAllForMilestone(repository.Id, milestone.Number)).Select(label => label.Name))
            {
                RepositoryIssueSectionRequest section;

                if (label == "enhancement")
                {
                    section = RepositoryIssueSectionRequestBuilder.CreateEnhancementRequest(milestone.Number);
                }
                else if (label == "bug")
                {
                    section = RepositoryIssueSectionRequestBuilder.CreateBugRequest(milestone.Number);
                }
                else if (label == "build")
                {
                    section = RepositoryIssueSectionRequestBuilder.CreateBuildRequest(milestone.Number);
                }
                else if (label == "documentation")
                {
                    section = RepositoryIssueSectionRequestBuilder.CreateDocumentationRequest(milestone.Number);
                }
                else
                {
                    section = RepositoryIssueSectionRequestBuilder.CreateRequest(milestone.Number, new SectionRequest { Emoji = label, Title = label, Label = label });
                }

                repositoryIssueSections.Add(section);
            }

            // Search Issue Section
            var searchIssueSections = new List<SearchIssueSectionRequest>
            {
                SearchIssueSectionRequestBuilder.CreateUnlabeledRequest(repository.FullName, milestone.Title)
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
