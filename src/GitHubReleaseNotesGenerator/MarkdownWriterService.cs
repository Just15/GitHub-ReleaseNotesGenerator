using System.Collections.Generic;
using System.Text;
using GitHubReleaseNotesGenerator.Models;
using Humanizer;
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

        public static void WriteReleaseNoteSections(StringBuilder stringBuilder, List<ReleaseNoteSectionResponse> sections)
        {
            foreach (var section in sections)
            {
                if (section.Issues.Count > 0)
                {
                    // Section Title
                    stringBuilder.AppendLine($"# {section.Emoji ?? section.DefultEmoji} {section.Title.Humanize(LetterCasing.Title)}");
                    stringBuilder.AppendLine();

                    // Section Issues
                    foreach (var issue in section.Issues)
                    {
                        stringBuilder.AppendLine($"* [#{issue.Number}]({issue.HtmlUrl}) {issue.Title}");
                    }
                    stringBuilder.AppendLine();
                }
            }
        }
    }
}
