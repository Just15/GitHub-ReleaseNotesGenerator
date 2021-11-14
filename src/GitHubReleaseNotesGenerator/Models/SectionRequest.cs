namespace GitHubReleaseNotesGenerator.Models
{
    public class SectionRequest : BaseReleaseNoteSection
    {
        public string Label { get; set; }

        public SectionRequest(string title, string label, string emoji)
        {
            Title = title;
            Label = label;
            Emoji = emoji;
        }

        public SectionRequest(string title, string label)
        {
            Title = title;
            Label = label;
            Emoji = label;
        }
    }
}
