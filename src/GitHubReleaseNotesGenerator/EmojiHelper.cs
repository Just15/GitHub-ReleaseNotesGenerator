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
            var containsKey = EmojiDictionary.TryGetValue(title, out string outEmoji);
            if (!containsKey)
            {
                outEmoji = string.Empty;
            }

            return outEmoji;
        }
    }
}
