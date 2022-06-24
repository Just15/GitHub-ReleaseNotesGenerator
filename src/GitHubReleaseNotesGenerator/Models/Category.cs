using System.Collections.Generic;

namespace GitHubReleaseNotesGenerator.Models
{
    public class Category
    {
        public string Title { get; set; }
        public List<string> Labels { get; set; }
    }
}
