using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesRequest : BaseReleaseNotes
    {
        public bool IncludeContributors { get; set; } = true;
        public bool IncludeUnlabeled { get; set; } = true;
        public List<ReleaseNoteSectionRequest> Sections { get; set; }
    }
}
