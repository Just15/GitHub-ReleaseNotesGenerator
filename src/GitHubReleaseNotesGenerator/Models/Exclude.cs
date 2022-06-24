using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubReleaseNotesGenerator.Models
{
    public class Exclude
    {
        public List<string> Labels { get; set; }
        public List<string> Authors { get; set; }
    }
}
