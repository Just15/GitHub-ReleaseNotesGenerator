using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

    // GitHub Action - https://github.com/marketplace/actions/release-notes-generator
    //      c# - https://github.com/Decathlon/release-notes-generator-action
    // Java - https://github.com/spring-io/github-changelog-generator

    public class GitHubReleaseNotesGenerator
    {
        private readonly string repositoryOwner;
        private readonly string repositoryName;
        private readonly GitHubClient gitHubClient;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials)
        {
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;

            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };
        }

        public async Task<ReleaseNotesResponse> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = new ReleaseNotesResponse { Miltestone = releaseNotesRequest.Miltestone };

            foreach (var section in releaseNotesRequest.Sections)
            {
                var issues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, section.RepositoryIssueRequest);
                response.Sections.Add(new ReleaseNoteSectionResponse { Image = section.Image, Title = section.Title, Issues = issues });
            }

            return response;
        }

        public async Task<int> GetMilestoneNumberFromTitle(string milestoneTitle)
        {
            var allMilestones = await gitHubClient.Issue.Milestone.GetAllForRepository(repositoryOwner, repositoryName);
            var milestone = allMilestones.Single(m => m.Title == milestoneTitle);
            return milestone.Number;
        }

        public async Task<ReleaseNotesRequest> CreateDefaultReleaseNotesRequest(string milestoneTitle)
        {
            var milestoneNumber = await GetMilestoneNumberFromTitle(milestoneTitle);

            // Enhancements
            var enhancementsSectionRequest = new ReleaseNoteSectionRequest
            {
                Image = "EnhancementsImage",
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString() }
            };
            enhancementsSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");

            // Bugs
            var bugsSectionRequest = new ReleaseNoteSectionRequest
            {
                Image = "BugsImage",
                Title = "Bugs",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString() }
            };
            bugsSectionRequest.RepositoryIssueRequest.Labels.Add("bug");

            // Request
            return new ReleaseNotesRequest
            {
                Miltestone = milestoneTitle,
                Sections = new List<ReleaseNoteSectionRequest>
                {
                    enhancementsSectionRequest,
                    bugsSectionRequest
                }
            };
        }
    }
}
