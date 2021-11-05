using System.Collections.Generic;
using Octokit;

namespace GitHubReleaseNotesGenerator.Models
{
    public class ReleaseNoteSectionResponse : BaseReleaseNoteSection
    {
        public IReadOnlyList<Issue> Issues { get; set; }
    }
}
