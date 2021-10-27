using Octokit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            HttpClient gitHubClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com/repos/octocat/")
            };

            // POST - /repos/{owner}/{repo}/releases/generate-notes
        }
    }
}
