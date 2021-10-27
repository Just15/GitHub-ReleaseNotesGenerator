namespace GitHubReleaseNotesGenerator
{
    public record GenerateNotes
    {
        public string TagName { get; init; }
        public string TargetCommitish { get; init; }
        public string PreviousTagName { get; init; }
        public string ConfigurationFilePath { get; init; }
    }
}
