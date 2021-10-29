using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNoteSectionRequest : BaseReleaseNoteSection
    {
        public RepositoryIssueRequest RepositoryIssueRequest { get; set; }
    }
}
