using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubReleaseNotesGenerator
{
    // https://octokitnet.readthedocs.io/en/latest/getting-started/

    // GitHub Action - https://github.com/marketplace/actions/release-notes-generator
    //      c# - https://github.com/Decathlon/release-notes-generator-action
    // Java - https://github.com/spring-io/github-changelog-generator

    public class GitHubReleaseNotesGenerator
    {
        private readonly string repositoryOwner;
        private readonly string repositoryName;
        private readonly GitHubClient gitHubClient;

        public GitHubReleaseNotesGenerator(string repositoryOwner, string repositoryName, Credentials credentials)
        {
            this.repositoryOwner = repositoryOwner;
            this.repositoryName = repositoryName;

            gitHubClient = new GitHubClient(new ProductHeaderValue(repositoryName))
            {
                Credentials = credentials
            };
        }

        public async Task<string> GenerateReleaseNotes(ReleaseNotesRequest releaseNotesRequest)
        {
            var milestones = await gitHubClient.Issue.Milestone.GetAllForRepository(repositoryOwner, repositoryName);

            var allIssues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName);

            return null;
        }

        public async Task GetRepositoryIssueRequest(RepositoryIssueRequest repositoryIssueRequest)
        {
            IReadOnlyList<Issue> issues = await gitHubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, repositoryIssueRequest);
        }

        public ReleaseNotesRequest CreateDefaultReleaseNotesRequest()
        {
            var defaultReleaseNotesRequest = new ReleaseNotesRequest();
            return defaultReleaseNotesRequest;
        }

        private void InitializeRepositoryIssueRequest()
        {
            RepositoryIssueRequest repositoryIssueRequest = new RepositoryIssueRequest();

            // RepositoryIssueRequest
            repositoryIssueRequest.Milestone = null;
            repositoryIssueRequest.Assignee = null;
            repositoryIssueRequest.Creator = null;
            repositoryIssueRequest.Mentioned = null;
            // IssueRequest
            repositoryIssueRequest.Filter = new IssueFilter();
            repositoryIssueRequest.State = new ItemStateFilter();
            repositoryIssueRequest.Labels.Add("");
            repositoryIssueRequest.SortProperty = new IssueSort();
            repositoryIssueRequest.SortDirection = new SortDirection();
            repositoryIssueRequest.Since = null;
        }
    }
}
