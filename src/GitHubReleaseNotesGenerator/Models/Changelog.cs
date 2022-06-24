using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator.Models
{
    public class Changelog
    {
        public Exclude Exclude { get; set; }
        public List<Category> Categories { get; set; }
    }
}
