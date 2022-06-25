using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator.Models
{
    public class Changelog
    {
        public List<Category> Categories { get; set; }
        public bool IncludeContributors { get; set; }
    }
}
