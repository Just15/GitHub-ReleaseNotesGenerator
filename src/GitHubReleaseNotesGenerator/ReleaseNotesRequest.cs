using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesRequest : BaseReleaseNotes
    {
        public bool IncludeContributors { get; set; }
        public List<ReleaseNoteSectionRequest> Sections { get; set; }
    }
}
