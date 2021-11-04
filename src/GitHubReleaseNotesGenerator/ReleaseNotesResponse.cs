using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNotesResponse : BaseReleaseNotes
    {
        public List<ReleaseNoteSectionResponse> Sections { get; set; } = new List<ReleaseNoteSectionResponse>();
    }
}
