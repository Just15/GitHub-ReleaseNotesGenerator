using Octokit;

namespace GitHubReleaseNotesGenerator.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
                "Just15",
                "PdfSharpWrapper",
                new Credentials(""));
        }
    }
}
