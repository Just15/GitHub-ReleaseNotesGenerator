# GitHubReleaseNotesGenerator

GitHubReleaseNotesGenerator is a tool to generate GitHub release notes based on labels and searches.

Example project: [Here](https://github.com/Just15/GitHubReleaseNotesGenerator/blob/main/src/GitHubReleaseNotesGenerator.ConsoleApp/Program.cs)

## Example Usage

Create release notes for default labels (Enhancements, Bugs, and Unlabeled).
```cs
GitHubReleaseNotesGenerator gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
    "[Repository Owner]", // Replace this
    "[Repository Name]", // Replace this
    "[Milestone Name]", // Replace this
    new Credentials("[GitHub Token]")); // Replace this

ReleaseNotesRequest defaultRequest = ReleaseNotesRequestBuilder.CreateDefault(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

string releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(defaultRequest);
```

Create release notes for a labels.
```cs
ReleaseNotesRequest allRequest = await ReleaseNotesRequestBuilder.CreateForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

string releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(allRequest);
```

Create a custom ```RepositoryIssueSectionRequest```.
```cs
RepositoryIssueSectionRequest customRepositoryIssueSectionRequest = new RepositoryIssueSectionRequest
{
    Emoji = "[Emoji]",
    Title = "[Section Title]",
    RepositoryIssueRequest = new RepositoryIssueRequest
    {
        // Milestone
        // Assignee
        // Creator
        // Mentioned
        // Filter
        // State
        // Labels
        // SortProperty
        // SortDirection
        // Since
    }
};
```

Create a custom ```SearchIssueSectionRequest```.
```cs
SearchIssueSectionRequest customSearchIssueSectionRequest = new SearchIssueSectionRequest
{
    Emoji = "[Emoji]",
    Title = "[Section Title]",
    SearchIssuesRequest = new SearchIssuesRequest
    {
        // Archived
        // User
        // Comments
        // Closed
        // Base
        // Head
        // Status
        // Merged
        // Updated
        // Created
        // Is
        // Language
        // No
        // Labels
        // State
        // Team
        // Involves
        // Commenter
        // Mentions
        // Assignee
        // Author
        // Involves
        // Type
        // Sort
        // SortField
        // Repos
        // Exclusions
    }
};
```

Override emojis used in section titles by setting the ```EmojiHelper.EmojiDictionary``` where the key is the label name and value is the emoji name.
```cs
EmojiHelper.EmojiDictionary = new Dictionary<string, string>
{
    { "enhancement", ":sunny:" },
    { "bug", ":worried:" },
    { "unlabeled", ":pencil2:" },
    { "help wanted", ":sos:" }
};
```

## :star: Powered By:
* [.NET](https://dotnet.microsoft.com/download)
* [NUKE Build](https://www.nuke.build/index.html)
* [Octokit](https://github.com/octokit/octokit.net)
* [Humanizr](https://humanizr.net/)
* [NUnit](https://nunit.org/)