using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesRequest : BaseReleaseNotes
    {
        public bool IncludeContributors { get; set; } = true;
        public List<RepositoryIssueSectionRequest> RepositoryIssueSections { get; set; }
        public List<SearchIssueSectionRequest> SearchIssueSections { get; set; }
    }
}
