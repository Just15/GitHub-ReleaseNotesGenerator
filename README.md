# GitHub-ReleaseNotesGenerator

GitHub-ReleaseNotesGenerator is a tool to generate GitHub release notes based on labels.

Example project: [Here](https://github.com/Just15/GitHubReleaseNotesGenerator/blob/main/src/GitHubReleaseNotesGenerator.ConsoleApp/Program.cs)

Example Release Notes : [Here](https://github.com/Just15/GitHubReleaseNotesGenerator/releases/tag/0.1.0)

## Getting Started

Specifiy repository owner, name, and GitHub credentials (if repository is private).

```csharp
GitHubReleaseNotesGenerator gitHubReleaseNotesGenerator = new GitHubReleaseNotesGenerator(
    "[Repository Owner]",
    "[Repository Name]",
    new Credentials("[GitHub Token]"));
```

Specifiy title and list of labels.

```csharp
var changelog = new Changelog
{
    Categories = new List<Category>
    {
        new Category
        {
            Title = ":star: Enhancement",
            Labels = new List<string> { "enhancement" },
        },
        new Category
        {
            Title = ":beetle: Bugs",
            Labels = new List<string> { "bug" },
        },
        new Category
        {
            Title = ":pushpin: No Label",
            Labels = null,
        },
    },
    IncludeContributors = true,
};
```

Generate release notes.

```csharp
var releaseNotes = await gitHubReleaseNotesGenerator.Generate("[Milestone Title]", changelog);
```

## :star: Powered By:
* [.NET](https://dotnet.microsoft.com/download)
* [NUKE Build](https://www.nuke.build/index.html)
* [Octokit](https://github.com/octokit/octokit.net)
* [NUnit](https://nunit.org/)
