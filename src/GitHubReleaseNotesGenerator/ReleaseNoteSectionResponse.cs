using Octokit;
using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator
{
    public class ReleaseNoteSectionResponse : BaseReleaseNoteSection
    {
        public IReadOnlyList<Issue> Issues { get; set; }
    }
}
