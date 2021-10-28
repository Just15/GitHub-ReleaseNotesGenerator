using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNoteSection
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public RepositoryIssueRequest RepositoryIssueRequest { get; set; }
    }
}
