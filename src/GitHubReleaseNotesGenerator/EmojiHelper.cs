using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public static class EmojiHelper
    {
        public static Dictionary<string, string> EmojiDictionary { get; set; } = new Dictionary<string, string>
        {
            { "enhancement", ":star:" },
            { "bug", ":beetle:" },
            { "unlabeled", ":pushpin:" },
            { "invalid", ":x:" },
            { "contributors", ":heart:" },
            { "build", ":wrench:" },
            { "help wanted", ":thought_balloon:" }
        };

        public static string TryGetEmoji(string title)
        {
            if (!EmojiDictionary.TryGetValue(title, out string outEmoji))
            {
                outEmoji = string.Empty;
            }

            return outEmoji;
        }
    }
}
