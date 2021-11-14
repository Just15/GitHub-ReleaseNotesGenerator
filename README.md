# GitHubReleaseNotesGenerator

GitHubReleaseNotesGenerator is a tool to generate GitHub release notes based on labels and searches.

Example project: [Here](https://github.com/Just15/GitHubReleaseNotesGenerator/blob/main/src/GitHubReleaseNotesGenerator.ConsoleApp/Program.cs)

Example Release Notes : [Here](https://github.com/Just15/GitHubReleaseNotesGenerator/releases/tag/0.1.0)

## Getting Started

Specifiy repository owner, name, milestone, and GitHub credentials.

```cs
GitHubReleaseNotesGenerator gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
    "[Repository Owner]",
    "[Repository Name]",
    "[Milestone Name]",
    new Credentials("[GitHub Token]"));
```

## Generating Release Notes

See below for specifying the release notes request.

```cs
string releaseNotes = await gitHubReleaseNotesGenerator.CreateReleaseNotes(new ReleaseNotesRequest());
```

## Default Requests

Create release notes for enhancement and bug labels and unlabeled issues.

```cs
ReleaseNotesRequest defaultRequest = ReleaseNotesRequestBuilder.CreateDefault(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

```

Create release notes for all labels and unlabeled issues.

```cs
ReleaseNotesRequest allLabelsRequest = await ReleaseNotesRequestBuilder.CreateForAllLabels(gitHubReleaseNotesGenerator.GitHubClient, gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone);

```

## Basic Request

Create a release note request specifying the sections you want. Sections can be custom by specifying the title and label (optional: emoji) or any built-in sections. 

```cs
ReleaseNotesRequest releaseNotesRequest = ReleaseNotesRequestBuilder.CreateCustom(gitHubReleaseNotesGenerator.Repository, gitHubReleaseNotesGenerator.Milestone, new List<SectionRequest>
{
    new SectionRequest("[Title]", "[Label]"),
    new SectionRequest("[Title]", "[Label]", "[Emoji]"),
    SectionRequestBuilder.CreateEnhancement(), // Built-in options
    SectionRequestBuilder.CreateBug(),
    SectionRequestBuilder.CreateBuild(),
    SectionRequestBuilder.CreateDocumentation(),
});
```

## Advanced Requests

Create a custom ```RepositoryIssueSectionRequest```.
```cs
RepositoryIssueSectionRequest customRepositoryIssueSectionRequest = new RepositoryIssueSectionRequest
{
    Title = "[Title]",
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
    },
    Emoji = "[Emoji]",
};
```

Create a custom ```SearchIssueSectionRequest```.
```cs
SearchIssueSectionRequest customSearchIssueSectionRequest = new SearchIssueSectionRequest
{
    Title = "[Title]",
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
    },
    Emoji = "[Emoji]",
};
```

## Emoji Support

Override emojis used in section titles by setting the ```EmojiHelper.EmojiDictionary``` where the key is the label name and value is the emoji name.
```cs
EmojiHelper.EmojiDictionary = new Dictionary<string, string>
{
    { "enhancement", ":star:" },
    { "bug", ":beetle:" },
    { "unlabeled", ":pushpin:" },
    { "documentation", ":book:" },
    { "invalid", ":x:" },
    { "contributors", ":heart:" },
    { "build", ":wrench:" },
    { "help wanted", ":thought_balloon:" }
};
```

## :star: Powered By:
* [.NET](https://dotnet.microsoft.com/download)
* [NUKE Build](https://www.nuke.build/index.html)
* [Octokit](https://github.com/octokit/octokit.net)
* [Humanizr](https://humanizr.net/)
* [NUnit](https://nunit.org/)
