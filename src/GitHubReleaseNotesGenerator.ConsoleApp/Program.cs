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
                new Credentials("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"));

            ReleaseNotesRequest releaseNotesRequest = new ReleaseNotesRequest();
            var response = await gitHubReleaseNotesGenerator.GenerateReleaseNotes(releaseNotesRequest);
        }
    }
}
