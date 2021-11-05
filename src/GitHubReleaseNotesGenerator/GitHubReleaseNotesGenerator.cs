using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class GitHubReleaseNotesGenerator
    {
        private readonly string repositoryOwner;
        private readonly string repositoryName;

        public GitHubClient GitHubClient { get; private set; }
        // TODO: Use this more and remove 'repositoryOwner' and 'repositoryName'
        public Repository Repository { get; private set; }
        public Milestone Milestone { get; private set; }

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, string milestoneTitle, Credentials credentials)
        {
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;

            GitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };

            Repository = GitHubClient.Repository.Get(repositoryOwner, repositoryName).Result;

            var allMilestones = GitHubClient.Issue.Milestone.GetAllForRepository(Repository.Id).Result;
            Milestone = allMilestones.Single(m => m.Title == milestoneTitle);
        }

        public async Task<ReleaseNotesResponse> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = new ReleaseNotesResponse { Milestone = releaseNotesRequest.Milestone };

            // Repository Issue Sections
            foreach (var section in releaseNotesRequest.RepositoryIssueSections)
            {
                var issues = await GitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, section.RepositoryIssueRequest);
                response.Sections.Add(new ReleaseNoteSectionResponse { Emoji = section.Emoji, DefultEmoji = section.DefultEmoji, Title = section.Title, Issues = issues });
            }

            // Search Issue Section
            foreach (var section in releaseNotesRequest.SearchIssueSections)
            {
                var searchIssueResult = await GitHubClient.Search.SearchIssues(section.SearchIssuesRequest);
                response.Sections.Add(new ReleaseNoteSectionResponse { Emoji = section.Emoji, DefultEmoji = section.DefultEmoji, Title = section.Title, Issues = searchIssueResult.Items });
            }

            return response;
        }

        public async Task<string> CreateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var stringBuilder = new StringBuilder();
            var response = await GenerateReleaseNotes(releaseNotesRequest);

            // Release Notes Section
            AddReleaseNoteSections(stringBuilder, response.Sections);

            // Contributors
            if (releaseNotesRequest.IncludeContributors)
            {
                var contributors = ContributorsService.GetContributors(response.Sections);
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
