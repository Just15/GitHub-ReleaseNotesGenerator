using Octokit;
using System;
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

        public async Task<string> CreateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var response = await GenerateReleaseNotes(releaseNotesRequest);

            var stringBuilder = new StringBuilder();
            foreach (var section in response.Sections)
            {
                if (section.Issues.Count > 0)
                {
                    stringBuilder.AppendLine($"# {GetSectionImage(section.Image)} {section.Title}");
                    stringBuilder.AppendLine();

                    foreach (var issue in section.Issues)
                    {
                        stringBuilder.AppendLine($"* {issue.Title} [#{issue.Number}]({issue.Url})");
                    }
                    stringBuilder.AppendLine();
                }
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
                Image = SectionImage.Star,
                Title = "Enhancements",
                RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString() }
            };
            enhancementsSectionRequest.RepositoryIssueRequest.Labels.Add("enhancement");

            // Bugs
            var bugsSectionRequest = new ReleaseNoteSectionRequest
            {
                Image = SectionImage.Bug,
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

        public async Task<ReleaseNotesRequest> CreateReleaseNotesForAllLabels(string milestoneTitle)
        {
            var milestoneNumber = await GetMilestoneNumberFromTitle(milestoneTitle);

            var sections = new List<ReleaseNoteSectionRequest>();
            foreach (var label in await gitHubClient.Issue.Labels.GetAllForMilestone(repositoryOwner, repositoryName, milestoneNumber))
            {
                var section = new ReleaseNoteSectionRequest
                {
                    Image = TryGetSectionImage(label.Name),
                    Title = label.Name,
                    RepositoryIssueRequest = new RepositoryIssueRequest { Milestone = milestoneNumber.ToString() }
                };
                section.RepositoryIssueRequest.Labels.Add(label.Name);

                sections.Add(section);
            }

            return new ReleaseNotesRequest
            {
                Miltestone = milestoneTitle,
                Sections = sections
            };
        }

        public static string GetSectionImage(SectionImage sectionImage) => sectionImage switch
        {
            SectionImage.NotSpecified or SectionImage.None => string.Empty,
            SectionImage.Star => "⭐",
            SectionImage.Bug => "🐞",
            SectionImage.Tool => "🔨",
            SectionImage.Heart => "❤️",
            _ => throw new NotImplementedException(),
        };

        public static SectionImage TryGetSectionImage(string title)
        {
            SectionImage sectionImage = SectionImage.NotSpecified;

            if (title.Contains("bug"))
            {
                sectionImage = SectionImage.Bug;
            }
            else if (title.Contains("enhancement"))
            {
                sectionImage = SectionImage.Star;
            }

            return sectionImage;
        }
    }
}
