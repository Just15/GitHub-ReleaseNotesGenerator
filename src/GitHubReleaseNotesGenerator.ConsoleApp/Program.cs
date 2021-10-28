using Octokit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

            HttpClient gitHubClient = new HttpClient
            {
                //BaseAddress = new Uri("https://api.github.com/repos/octocat/")
                BaseAddress = new Uri("https://api.github.com/repos/"),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", "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")
                }
            };

            await gitHubReleaseNotesGenerator.DoStuff();
        }
    }
}
