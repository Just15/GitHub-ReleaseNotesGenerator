using System.Collections.Generic;
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
            Milestone = GitHubClient.Issue.Milestone.GetAllForRepository(Repository.Id).Result.Single(m => m.Title == milestoneTitle);
        }

        public async Task<ReleaseNotesResponse> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = new ReleaseNotesResponse { Milestone = releaseNotesRequest.Milestone };

            response.Sections.AddRange(await ReleaseNoteSectionResponseBuilder.Create(GitHubClient, Repository, releaseNotesRequest.RepositoryIssueSections));
            response.Sections.AddRange(await ReleaseNoteSectionResponseBuilder.Create(GitHubClient, releaseNotesRequest.SearchIssueSections));

            return response;
        }

        public async Task<string> CreateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var releaseNotesResponse = await GenerateReleaseNotes(releaseNotesRequest);

            var stringBuilder = new StringBuilder();
            AddReleaseNoteSections(stringBuilder, releaseNotesResponse.Sections);

            // Contributors
            if (releaseNotesRequest.IncludeContributors)
            {
                var contributors = ContributorsService.GetContributors(releaseNotesResponse.Sections);
                MarkdownWriterService.WriteUsers(stringBuilder, contributors, $"# :heart: Contributors", "## We'd like to thank all the contributors who worked on this release!");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static void AddReleaseNoteSections(StringBuilder stringBuilder, List<ReleaseNoteSectionResponse> sections)
        {
            foreach (var section in sections)
            {
                if (section.Issues.Count > 0)
                {
                    // Section Title
                    stringBuilder.AppendLine($"# {section.Emoji ?? section.DefultEmoji} {section.Title}");
                    stringBuilder.AppendLine();

                    // Section Issues
                    foreach (var issue in section.Issues)
                    {
                        stringBuilder.AppendLine($"* [#{issue.Number}]({issue.HtmlUrl}) {issue.Title}");
                    }
                    stringBuilder.AppendLine();
                }
            }
        }
    }
}
