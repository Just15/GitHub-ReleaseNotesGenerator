using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class GitHubReleaseNotesGenerator
    {
        public GitHubClient GitHubClient { get; private set; }
        public Repository Repository { get; private set; }
        public Milestone Milestone { get; private set; }

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, string milestoneTitle, Credentials credentials)
        {
            GitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };

            Repository = GitHubClient.Repository.Get(repositoryOwner, repositoryName).Result;
            Milestone = GitHubClient.Issue.Milestone.GetAllForRepository(Repository.Id).Result.SingleOrDefault(m => m.Title == milestoneTitle);
            if (Milestone == null)
            {
                throw new ArgumentException($"A milestone with name '{milestoneTitle}' doesn't exist.");
            }
        }

        public async Task<ReleaseNotesResponse> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = new ReleaseNotesResponse { Milestone = releaseNotesRequest.Milestone };

            response.Sections.AddRange(await ReleaseNoteSectionResponseService.Create(GitHubClient, Repository, releaseNotesRequest.RepositoryIssueSections));
            response.Sections.AddRange(await ReleaseNoteSectionResponseService.Create(GitHubClient, releaseNotesRequest.SearchIssueSections));

            return response;
        }

        public async Task<string> CreateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var releaseNotesResponse = await GenerateReleaseNotes(releaseNotesRequest);

            var stringBuilder = new StringBuilder();
            MarkdownWriterService.WriteReleaseNoteSections(stringBuilder, releaseNotesResponse.Sections);

            // Contributors
            if (releaseNotesRequest.IncludeContributors)
            {
                var contributors = ContributorsService.GetContributors(releaseNotesResponse.Sections);
                MarkdownWriterService.WriteUsers(stringBuilder, contributors, $"# :heart: Contributors", "## We'd like to thank all the contributors who worked on this release!");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
