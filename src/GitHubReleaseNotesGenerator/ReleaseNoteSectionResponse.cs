using System.Collections.Generic;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNoteSectionResponse : BaseReleaseNoteSection
    {
        public IReadOnlyList<Issue> Issues { get; set; }
    }
}
