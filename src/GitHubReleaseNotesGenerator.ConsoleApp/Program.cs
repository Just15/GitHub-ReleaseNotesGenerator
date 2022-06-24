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
            var gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
                "Just15",
                "GitVersion-PdfSharpWrapper",
                new Credentials("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"));

            var changelog = new Changelog
            {
                Categories = new List<Category>
                {
                    new Category
                    {
                        Title = ":star: Enhancement",
                        Labels = new List<string> { "enhancement" },
                    },
                    new Category
                    {
                        Title = ":beetle: Bugs",
                        Labels = new List<string> { "bug" },
                    },
                    new Category
                    {
                        Title = ":pushpin: No Label",
                        Labels = null,
                    },
                }
            };

            var releaseNotes = await gitHubReleaseNotesGenerator.Generate("Milestone 2", changelog);

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReleaseNotesTest.md");
            File.WriteAllText(filePath, releaseNotes);
        }
    }
}
