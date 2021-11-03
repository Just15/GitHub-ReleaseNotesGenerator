using Octokit;
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

            var response = await gitHubReleaseNotesGenerator.GenerateReleaseNotes(allRequest);
        }
    }
}
