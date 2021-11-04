using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    public class GitHubReleaseNotesGenerator
    {
        private readonly string repositoryOwner;
        private readonly string repositoryName;
        // TODO: Use this more
        private readonly Repository repository;
        private readonly GitHubClient gitHubClient;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials)
        {
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;

            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };

            repository = gitHubClient.Repository.Get(repositoryOwner, repositoryName).Result;
        }

        public async Task<ReleaseNotesResponse> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = new ReleaseNotesResponse { Milestone = releaseNotesRequest.Milestone };

            // Repository Issue Sections
            foreach (var section in releaseNotesRequest.RepositoryIssueSections)
            {
                var issues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, section.RepositoryIssueRequest);
                response.Sections.Add(new ReleaseNoteSectionResponse { Emoji = section.Emoji, DefultEmoji = section.DefultEmoji, Title = section.Title, Issues = issues });
            }

            // Search Issue Section
            foreach (var section in releaseNotesRequest.SearchIssueSections)
            {
                var searchIssueResult = await gitHubClient.Search.SearchIssues(section.SearchIssuesRequest);
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
            var enhancementsSectionRequest = new RepositoryIssueSectionRequest
            {
                DefultEmoji = ":star:",
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
            };
            enhancementsSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");

            // Bugs
            var bugsSectionRequest = new RepositoryIssueSectionRequest
            {
                DefultEmoji = ":beetle:",
                Title = "Bugs",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
            };
            bugsSectionRequest.RepositoryIssueRequest.Labels.Add("bug");

            // Unlabeled
            var unlabeledSectionRequest = new SearchIssueSectionRequest
            {
                DefultEmoji = ":pushpin:",
                Title = "Unlabeled",
                SearchIssuesRequest = new SearchIssuesRequest { Repos = new RepositoryCollection { repository.FullName }, Milestone = milestoneTitle, No = IssueNoMetadataQualifier.Label }
            };

            // Request
            return new ReleaseNotesRequest
            {
                Milestone = milestoneTitle,
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

        public async Task<ReleaseNotesRequest> CreateReleaseNotesForAllLabels(string milestoneTitle)
        {
            var milestoneNumber = await GetMilestoneNumberFromTitle(milestoneTitle);

            // Repository Issue Sections
            var repositoryIssueSections = new List<RepositoryIssueSectionRequest>();
            foreach (var label in await gitHubClient.Issue.Labels.GetAllForMilestone(repositoryOwner, repositoryName, milestoneNumber))
            {
                var section = new RepositoryIssueSectionRequest
                {
                    DefultEmoji = EmojiHelper.TryGetEmoji(label.Name),
                    Title = label.Name,
                    RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
                };
                section.RepositoryIssueRequest.Labels.Add(label.Name);

                repositoryIssueSections.Add(section);
            }

            // Search Issue Section
            var searchIssueSections = new List<SearchIssueSectionRequest>
            {
                new SearchIssueSectionRequest
                {
                    DefultEmoji = ":pushpin:",
                    Title = "Unlabeled",
                    SearchIssuesRequest = new SearchIssuesRequest { Repos = new RepositoryCollection { repository.FullName }, Milestone = milestoneTitle, No = IssueNoMetadataQualifier.Label }
                }
            };

            return new ReleaseNotesRequest
            {
                Milestone = milestoneTitle,
                RepositoryIssueSections = repositoryIssueSections,
                SearchIssueSections = searchIssueSections
            };
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
