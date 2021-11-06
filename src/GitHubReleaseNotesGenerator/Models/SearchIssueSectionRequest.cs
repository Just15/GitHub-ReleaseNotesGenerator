using Octokit;

namespace GitHubReleaseNotesGenerator.Models
{
    public class SearchIssueSectionRequest : BaseReleaseNoteSection
    {
        public SearchIssuesRequest SearchIssuesRequest { get; set; }
    }
}
