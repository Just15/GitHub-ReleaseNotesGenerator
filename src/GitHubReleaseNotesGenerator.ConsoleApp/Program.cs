using System.Collections.Generic;
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
                "[Repository Owner]",
                "[Repository Name]",
                new Credentials("[GitHub Token]"));

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
                },
                IncludeContributors = true,
            };

            var releaseNotes = await gitHubReleaseNotesGenerator.Generate("[Milestone Title]", changelog);
        }
    }
}
