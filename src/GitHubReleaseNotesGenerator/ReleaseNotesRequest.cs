using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesRequest : BaseReleaseNotes
    {
        public List<ReleaseNoteSectionRequest> Sections { get; set; }
    }
}
