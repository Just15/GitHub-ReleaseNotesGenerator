namespace GitHubReleaseNotesGenerator
{
    public static class EmojiHelper
    {
        public static string TryGetEmoji(string title)
        {
            string emoji = string.Empty;

            // TODO: Change to dictionary that can be overridden

            if (title.Contains("enhancement"))
            {
                emoji = ":star:";
            }
            if (title.Contains("bug"))
            {
                emoji = ":beetle:";
            }
            else if (title.Contains("unlabeled"))
            {
                emoji = ":pushpin:";
            }
            else if (title.Contains("contributors"))
            {
                emoji = ":heart:";
            }
            else if (title.Contains("build"))
            {
                emoji = ":wrench:";
            }
            else if (title.Contains("help"))
            {
                emoji = ":thought_balloon:";
            }

            return emoji;
        }
    }
}
