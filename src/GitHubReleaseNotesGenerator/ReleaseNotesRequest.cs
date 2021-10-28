using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesRequest
    {
        public List<ReleaseNoteSection> Sections { get; set; }
    }
}
