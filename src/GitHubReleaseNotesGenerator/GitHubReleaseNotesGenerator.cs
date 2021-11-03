﻿using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

    // GitHub Action - https://github.com/marketplace/actions/release-notes-generator
    //      c# - https://github.com/Decathlon/release-notes-generator-action
    // Java - https://github.com/spring-io/github-changelog-generator

    // https://www.webfx.com/tools/emoji-cheat-sheet/
    // https://github.com/markdown-templates/markdown-emojis

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

            foreach (var section in releaseNotesRequest.Sections)
            {
                var issues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, section.RepositoryIssueRequest);
                response.Sections.Add(new ReleaseNoteSectionResponse { Emoji = section.Emoji, Title = section.Title, Issues = issues });
            }

            return response;
        }

        public async Task<string> CreateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var stringBuilder = new StringBuilder();
            var response = await GenerateReleaseNotes(releaseNotesRequest);

            // Sections
            AddReleaseNoteSections(stringBuilder, response.Sections);

            // Unlabeled
            if (releaseNotesRequest.IncludeUnlabeled)
            {
                var unlabeledIssues = await gitHubClient.Search.SearchIssues(new SearchIssuesRequest
                {
                    Repos = new RepositoryCollection { repository.FullName },
                    Milestone = releaseNotesRequest.Milestone,
                    No = IssueNoMetadataQualifier.Label,
                });
                AddSearchIssuesResult(stringBuilder, unlabeledIssues, "unlabeled", "unlabeled");
            }

            // Contributors
            if (releaseNotesRequest.IncludeContributors)
            {
                var contributors = GetContributors(response);
                AddContributors(stringBuilder, contributors);
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
            var enhancementsSectionRequest = new ReleaseNoteSectionRequest
            {
                Emoji = ":star:",
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
            };
            enhancementsSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");

            // Bugs
            var bugsSectionRequest = new ReleaseNoteSectionRequest
            {
                Emoji = ":beetle:",
                Title = "Bugs",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
            };
            bugsSectionRequest.RepositoryIssueRequest.Labels.Add("bug");

            // Request
            return new ReleaseNotesRequest
            {
                Milestone = milestoneTitle,
                Sections = new List<ReleaseNoteSectionRequest>
                {
                    enhancementsSectionRequest,
                    bugsSectionRequest
                }
            };
        }

        public async Task<ReleaseNotesRequest> CreateReleaseNotesForAllLabels(string milestoneTitle)
        {
            var milestoneNumber = await GetMilestoneNumberFromTitle(milestoneTitle);

            var sections = new List<ReleaseNoteSectionRequest>();
            foreach (var label in await gitHubClient.Issue.Labels.GetAllForMilestone(repositoryOwner, repositoryName, milestoneNumber))
            {
                var section = new ReleaseNoteSectionRequest
                {
                    Emoji = TryGetEmoji(label.Name),
                    Title = label.Name,
                    RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString(), State = ItemStateFilter.Closed }
                };
                section.RepositoryIssueRequest.Labels.Add(label.Name);

                sections.Add(section);
            }

            return new ReleaseNotesRequest
            {
                Milestone = milestoneTitle,
                Sections = sections
            };
        }

        public static List<User> GetContributors(ReleaseNotesResponse releaseNotesResponse)
        {
            var contributors = new List<User>();
            foreach (var section in releaseNotesResponse.Sections)
            {
                foreach (var issue in section.Issues)
                {
                    if (issue.Assignee != null &&
                        contributors.SingleOrDefault(c => c.Name == issue.Assignee.Name) == null)
                    {
                        contributors.Add(issue.Assignee);
                    }
                }
            }

            return contributors;
        }

        public static void AddReleaseNoteSections(StringBuilder stringBuilder, List<ReleaseNoteSectionResponse> sections)
        {
            foreach (var section in sections)
            {
                if (section.Issues.Count > 0)
                {
                    // Section Title
                    stringBuilder.AppendLine($"# {section.Emoji} {section.Title}");
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

        public static void AddSearchIssuesResult(StringBuilder stringBuilder, SearchIssuesResult searchIssuesResult, string title, string emoji)
        {
            if (searchIssuesResult.TotalCount > 0)
            {
                stringBuilder.AppendLine($"# {TryGetEmoji(emoji)} {title}");
                stringBuilder.AppendLine();

                foreach (var issue in searchIssuesResult.Items)
                {
                    stringBuilder.AppendLine($"* [#{issue.Number}]({issue.HtmlUrl}) {issue.Title}");
                }
                stringBuilder.AppendLine();
            }
        }

        public static void AddContributors(StringBuilder stringBuilder, List<User> users)
        {
            if (users.Count > 0)
            {
                stringBuilder.AppendLine($"# :heart: Contributors");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("## We'd like to thank all the contributors who worked on this release!");
                stringBuilder.AppendLine();

                foreach (var user in users)
                {
                    stringBuilder.AppendLine($"* [{user.Name ?? user.Login}]({user.HtmlUrl})");
                }
            }
        }

        public static string TryGetEmoji(string title)
        {
            string emoji = string.Empty;

            if (title.Contains("enhancement"))
            {
                emoji = ":star:";
            }
            if (title.Contains("bug"))
            {
                emoji = ":beetle:";
            }
            else if (title.Contains("unlabeled"))
            {
                emoji = ":pushpin:";
            }
            else if (title.Contains("contributors"))
            {
                emoji = ":heart:";
            }
            else if (title.Contains("build"))
            {
                emoji = ":wrench:";
            }
            else if (title.Contains("help"))
            {
                emoji = ":sos:";
            }

            return emoji;
        }
    }
}
