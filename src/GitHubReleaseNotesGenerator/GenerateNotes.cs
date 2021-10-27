namespace GitHubReleaseNotesGenerator
{
    public record GenerateNotes
    {
        public string Owner { get; init; }
        public string Repo { get; init; }

        public string TagName { get; init; }
    }
}
