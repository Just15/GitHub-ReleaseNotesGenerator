using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    internal static class MarkdownGenerator
    {
        internal static string GenerateIssueText(List<(string Title, SearchIssuesResult SearchIssuesResult)> results)
        {
            var stringBuilder = new StringBuilder();

            foreach (var result in results)
            {
                if (result.SearchIssuesResult.TotalCount > 0)
                {
                    stringBuilder.AppendLine($"# {result.Title}");
                    stringBuilder.AppendLine();

                    foreach (var item in result.SearchIssuesResult.Items)
                    {
                        stringBuilder.AppendLine($"* [#{item.Number}]({item.HtmlUrl}) {item.Title}");
                    }
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }

        internal static string GenerateContributorsText(List<(string Title, SearchIssuesResult SearchIssuesResult)> results)
        {
            var users = new List<User>();
            foreach (var result in results)
            {
                foreach (var item in result.SearchIssuesResult.Items)
                {
                    foreach (var assignee in item.Assignees)
                    {
                        if (users.SingleOrDefault(c => c.Name == assignee.Name) == null)
                        {
                            users.Add(assignee);
                        }
                    }
                }
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("# :heart: Contributors");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("## We'd like to thank all the contributors who worked on this release!");
            stringBuilder.AppendLine();
            foreach (var user in users)
            {
                stringBuilder.AppendLine($"* [{user.Name ?? user.Login}]({user.HtmlUrl})");
            }
            return stringBuilder.ToString();
        }
    }
}
