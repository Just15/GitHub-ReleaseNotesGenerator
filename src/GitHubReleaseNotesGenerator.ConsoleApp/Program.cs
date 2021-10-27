using Octokit;
using System;
using System.Net.Http;

namespace GitHubReleaseNotesGenerator.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HttpClient gitHubClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com/repos/octocat/")
            };
        }
    }
}
