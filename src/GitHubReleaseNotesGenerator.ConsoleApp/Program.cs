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
            string tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GitHubToken.txt");
            string gitHubToken = File.ReadAllText(tokenFilePath);
            var gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
                "Just15",
                "GitVersion-PdfSharpWrapper",
                "Milestone 2",
                new Credentials(gitHubToken));

            var request = ReleaseNotesRequestBuilder.CreateCustom(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone, new List<SectionRequest>
            {
                new SectionRequest { Emoji = "[Repalce]", Title = "[Repalce]", Label = "[Repalce]" },
            });
            var defaultRequest = ReleaseNotesRequestBuilder.CreateDefault(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);
            var allRequest = await ReleaseNotesRequestBuilder.CreateForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

            // Write release notes to file
            string tempFile = "ReleaseNotes.md";
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(allRequest);
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
