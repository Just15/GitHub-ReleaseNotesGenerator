using GitHubReleaseNotesGenerator.Models;

namespace GitHubReleaseNotesGenerator
{
    public static class SectionRequestBuilder
    {
        public static SectionRequest CreateEnhancement()
        {
            return new SectionRequest("Enhancements", "enhancement");
        }

        public static SectionRequest CreateBug()
        {
            return new SectionRequest("Bugs", "bug");
        }

        public static SectionRequest CreateBuild()
        {
            return new SectionRequest("Build", "build");
        }

        public static SectionRequest CreateDocumentation()
        {
            return new SectionRequest("Documentation", "documentation");
        }
    }
}
