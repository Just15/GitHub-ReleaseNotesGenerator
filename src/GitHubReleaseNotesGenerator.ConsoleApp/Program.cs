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
                "Milestone 2",
                new Credentials("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"));

            var defaultRequest = ReleaseNotesRequestBuilder.CreateDefault(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);
            var allRequest = await ReleaseNotesRequestBuilder.CreateForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

            // Write release notes to file
            string tempFile = "ReleaseNotes.md";
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(defaultRequest);
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
