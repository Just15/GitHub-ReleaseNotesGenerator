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
                new Credentials(""));

            var defaultRequest = ReleaseNotesRequestBuilder.CreateDefaultReleaseNotesRequest(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);
            var allRequest = await ReleaseNotesRequestBuilder.CreateReleaseNotesForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

            // Write release notes to file
            string tempFile = "ReleaseNotes.md";
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(defaultRequest);
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
