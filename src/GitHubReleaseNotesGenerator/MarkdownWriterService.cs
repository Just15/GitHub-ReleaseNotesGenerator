using System.Collections.Generic;
using System.Text;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class MarkdownWriterService
    {
        public static void WriteUsers(StringBuilder stringBuilder, List<User> users, string header, string text)
        {
            if (users.Count > 0)
            {
                stringBuilder.AppendLine(header);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(text);
                stringBuilder.AppendLine();

                foreach (var user in users)
                {
                    stringBuilder.AppendLine($"* [{user.Name ?? user.Login}]({user.HtmlUrl})");
                }
            }
        }
    }
}
