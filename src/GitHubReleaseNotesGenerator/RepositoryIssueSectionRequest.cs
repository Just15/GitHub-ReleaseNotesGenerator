using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class RepositoryIssueSectionRequest : BaseReleaseNoteSection
    {
        public RepositoryIssueRequest RepositoryIssueRequest { get; set; }
    }
}
