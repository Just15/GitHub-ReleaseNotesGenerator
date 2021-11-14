using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GitHubReleaseNotesGenerator.Models;
using Octokit;

namespace GitHubReleaseNotesGenerator.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // Get Credentials
            string tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GitHubToken.txt");
            string gitHubToken = File.ReadAllText(tokenFilePath);

            // Getting Started
            var gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
                "[Repository Owner]",
                "[Repository Name]",
                "[Milestone Title]",
                new Credentials("[GitHub Token]"));

            // Default Request
            var defaultRequest = ReleaseNotesRequestBuilder.CreateDefault(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

            // All Labels Request
            var allLabelsRequest = await ReleaseNotesRequestBuilder.CreateForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

            // Basic Request
            var basicRequest = ReleaseNotesRequestBuilder.CreateCustom(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone, new List<SectionRequest>
            {
                new SectionRequest("[Title]", "[Label]"),
                new SectionRequest("[Title]", "[Label]", "[Emoji]"),
                SectionRequestBuilder.CreateBug()
            });

            // Advanced Request
            var advancedRequest = new ReleaseNotesRequest
            {
                Milestone = gitHubReleaseNotesGenerator.Milestone.Title,
                RepositoryIssueSections = new List<RepositoryIssueSectionRequest>
                {
                    new RepositoryIssueSectionRequest
                    {
                        Title = "[Title]",
                        RepositoryIssueRequest = new RepositoryIssueRequest
                        {
                            // Specify options
                        },
                        Emoji = "[Emoji]",
                    }
                },
                SearchIssueSections = new List<SearchIssueSectionRequest>
                {
                    new SearchIssueSectionRequest
                    {
                        Title = "[Title]",
                        SearchIssuesRequest = new SearchIssuesRequest
                        {
                            // Specify options
                        },
                        Emoji = "[Emoji]",
                    }
                }
            };

            // Generating Release Notes
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(defaultRequest);

            // Write Release Notes to File
            string tempFile = "ReleaseNotes.md";
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
