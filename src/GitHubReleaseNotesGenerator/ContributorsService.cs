using Octokit;
using System.Collections.Generic;
using System.Linq;

namespace GitHubReleaseNotesGenerator
{
    public class ContributorsService
    {
        public static List<User> GetContributors(List<ReleaseNoteSectionResponse> sections)
        {
            var contributors = new List<User>();

            foreach (var section in sections)
            {
                foreach (var issue in section.Issues)
                {
                    if (issue.Assignee != null &&
                        contributors.SingleOrDefault(c => c.Name == issue.Assignee.Name) == null)
                    {
                        contributors.Add(issue.Assignee);
                    }
                }
            }

            return contributors;
        }
    }
}
