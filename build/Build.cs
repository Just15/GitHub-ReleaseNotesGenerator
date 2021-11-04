using Microsoft.AspNetCore.StaticFiles;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Octokit;
using System.IO;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;
    [GitRepository] readonly GitRepository GitRepository;

    [Parameter] readonly string GitHubAuthenticationToken;
    [Parameter] readonly string Source = "https://api.nuget.org/v3/index.json";
    [Parameter] readonly string SymbolSource = "https://nuget.smbsrc.net/";
    [Parameter] readonly string NugetApiKey;
    Release createdRelease;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .TriggeredBy(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject(Solution.GitHubReleaseNotesGenerator))
                .SetVersion(GitVersion.MajorMinorPatch)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .EnableIncludeSymbols()
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableContinuousIntegrationBuild());
        });

    Target CreateGitHubRelease => _ => _
        .DependsOn(Pack)
        .Requires(() => GitHubAuthenticationToken)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(async () =>
        {
            ControlFlow.Assert(GitVersion.BranchName.Equals("master"), "Branch isn't 'master'.");

            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(nameof(NukeBuild)))
            {
                Credentials = new Credentials(GitHubAuthenticationToken)
            };

            var newRelease = new NewRelease(GitVersion.MajorMinorPatch)
            {
                TargetCommitish = GitVersion.Sha,
                Name = GitVersion.MajorMinorPatch,
                Body = "Add release notes...",
                Draft = true,
            };

            createdRelease = await GitHubTasks.GitHubClient.Repository.Release.Create(GitRepository.GetGitHubOwner(), GitRepository.GetGitHubName(), newRelease);
        });

    Target UploadReleaseAssetsToGithub => _ => _
        .TriggeredBy(CreateGitHubRelease)
        .Unlisted()
        .Executes(() =>
        {
            ControlFlow.NotNull(createdRelease, $"'createdRelease' is null.");

            GlobFiles(ArtifactsDirectory, "*.nupkg", "*.snupkg")
                .NotEmpty()
                .ForEach(x =>
                {
                    if (!new FileExtensionContentTypeProvider().TryGetContentType(x, out var assetContentType))
                    {
                        assetContentType = "application/x-binary";
                    }

                    var releaseAssetUpload = new ReleaseAssetUpload
                    {
                        ContentType = assetContentType,
                        FileName = Path.GetFileName(x),
                        RawData = File.OpenRead(x)
                    };

                    var releaseAsset = GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload).Result;
                });
        });

    Target UploadNuGetPackage => _ => _
        .Requires(() => Source)
        .Requires(() => SymbolSource)
        .Requires(() => NugetApiKey)
        .TriggeredBy(CreateGitHubRelease)
        .Unlisted()
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory, "*.nupkg", "*.snupkg")
                .NotEmpty()
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(Source)
                        .SetSymbolSource(SymbolSource)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });
}
