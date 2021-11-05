using Octokit;

namespace GitHubReleaseNotesGenerator.Models
{
    public class RepositoryIssueSectionRequest : BaseReleaseNoteSection
    {
        public RepositoryIssueRequest RepositoryIssueRequest { get; set; }
    }
}
