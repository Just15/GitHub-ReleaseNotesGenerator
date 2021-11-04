using Octokit;
using System.IO;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
                "Just15",
                "GitVersion-PdfSharpWrapper",
                new Credentials(""));

            var defaultRequest = await gitHubReleaseNotesGenerator.CreateDefaultReleaseNotesRequest("Milestone 2");
            var allRequest = await gitHubReleaseNotesGenerator.CreateReleaseNotesForAllLabels("Milestone 2");

            // Write release notes to file
            string tempFile = "ReleaseNotes.md";
            var releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(allRequest);
            File.WriteAllText(tempFile, releaseNotes);
        }
    }
}
