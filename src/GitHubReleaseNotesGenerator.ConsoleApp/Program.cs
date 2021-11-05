using System.IO;
using System.Threading.Tasks;
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

            var defaultRequest = await gitHubReleaseNotesGenerator.CreateDefaultReleaseNotesRequest("Milestone 2");
            var allRequest = await gitHubReleaseNotesGenerator.CreateReleaseNotesForAllLabels("Milestone 2");

            // Write release notes to file
            string tempFile = "ReleaseNotes.md";
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(allRequest);
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
