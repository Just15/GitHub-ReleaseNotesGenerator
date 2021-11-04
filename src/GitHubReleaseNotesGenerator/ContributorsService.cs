using System.Collections.Generic;
using System.Linq;
using Octokit;

namespace GitHubReleaseNotesGenerator
{
    public static class ContributorsService
    {
        public static List<User> GetContributors(List<ReleaseNoteSectionResponse> sections)
        {
            var contributors = new List<User>();

            foreach (var section in sections)
            {
                foreach (var assignee in section.Issues.Select(issue => issue.Assignee))
                {
                    if (contributors.SingleOrDefault(c => c.Name == assignee.Name) == null)
                    {
                        contributors.Add(assignee);
                    }
                }
            }

            return contributors;
        }
    }
}
