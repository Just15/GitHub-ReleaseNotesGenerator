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
                "PdfSharpWrapper",
                new Credentials(""));

            await gitHubReleaseNotesGenerator.GenerateReleaseNotes();
        }
    }
}
