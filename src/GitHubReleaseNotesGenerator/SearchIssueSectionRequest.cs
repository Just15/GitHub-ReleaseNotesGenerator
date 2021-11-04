using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class SearchIssueSectionRequest : BaseReleaseNoteSection
    {
        public SearchIssuesRequest SearchIssuesRequest { get; set; }
    }
}
