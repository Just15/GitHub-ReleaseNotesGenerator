using System.Text;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class MarkdownGenerator
    {
        public static void GenerateIssueText(StringBuilder stringBuilder, string title, SearchIssuesResult searchIssuesResult)
        {
            if (searchIssuesResult.TotalCount > 0)
            {
                stringBuilder.AppendLine($"# {title}");
                stringBuilder.AppendLine();

                foreach (var item in searchIssuesResult.Items)
                {
                    stringBuilder.AppendLine($"* [#{item.Number}]({item.HtmlUrl}) {item.Title}");
                }
                stringBuilder.AppendLine();
            }
        }
    }
}
