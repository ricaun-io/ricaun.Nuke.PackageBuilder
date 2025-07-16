using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.InnoSetup;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components;

/// <summary>
/// IAutoCADPackageBuilder
/// </summary>
public interface IAutoCADPackageBuilder : IAutoCADPackageBuilder<IssAutoCADBuilder>
{

}

/// <summary>
/// IAutoCADPackageBuilder
/// </summary>
public interface IAutoCADPackageBuilder<T> :
    IHazAutoCADPackageBuilder, IHazPackageBuilderProject, IHazInstallationFiles,
    IRelease, ISign, IHazPackageBuilder, IHazInput, IHazOutput, INukeBuild
    where T : IssPackageBuilder, new()
{
    /// <summary>
    /// Target PackageBuilder
    /// </summary>
    Target PackageBuilder => _ => _
        .TriggeredBy(Sign)
        .Before(Release)
        .Executes(() =>
        {
            Project packageBuilderProject = GetPackageBuilderProject();
            if (GetMainProject() != packageBuilderProject)
                Solution.BuildProject(packageBuilderProject, (project) =>
                {
                    SignProject(project);
                    project.ShowInformation();
                }
                );

            CreatePackageBuilder(packageBuilderProject, ReleasePackageBuilder, ReleaseBundle);
        });

    /// <summary>
    /// CreatePackageBuilder
    /// </summary>
    /// <param name="project"></param>
    /// <param name="releasePackageBuilder"></param>
    /// <param name="releaseBundle"></param>
    public void CreatePackageBuilder(Project project, bool releasePackageBuilder = false, bool releaseBundle = false)
    {
        var projectName = project.Name;
        var projectVersion = project.GetInformationalVersion();

        if (projectVersion is null)
        {
            throw new Exception($"Project {projectName} has no version, assembly file not found. Make sure your project name is the same as the assembly file name.");
        }

        var projectNameVersion = GetReleaseFileNameVersion(projectName, projectVersion);

        var bundleName = $"{projectName}.bundle";
        var BundleDirectory = PackageBuilderDirectory / bundleName;
        var ContentsDirectory = BundleDirectory / "Contents";

        if (ProjectNameFolder)
            ContentsDirectory = ContentsDirectory / projectName;

        if (ProjectVersionFolder)
            ContentsDirectory = ContentsDirectory / projectVersion;

        AbsolutePathExtensions.Copy(InputDirectory, ContentsDirectory);

        if (ProjectRemoveTargetFrameworkFolder)
        {
            AppendTargetFrameworkExtension.RemoveAppendTargetFrameworkDirectory(ContentsDirectory);
        }

        var autoCADFiles = CreateAutoCADAddinOnProjectFiles(project, ContentsDirectory);

        new AutoCADContentsBuilder(project, BundleDirectory, autoCADFiles, MiddleVersions, NewVersions)
            .Build(BundleDirectory / "PackageContents.xml");

        if (releasePackageBuilder)
        {
            // CopyInstallationFiles If Exists
            CopyInstallationFilesTo(PackageBuilderDirectory);

            // Create Iss Files
            try
            {
                Serilog.Log.Information($"IssPackageBuilder: {typeof(T)}");
                var issPackageBuilder = new T();
                issPackageBuilder
                    .Initialize(project)
                    .CreatePackage(PackageBuilderDirectory, IssConfiguration)
                    .CreateFile(PackageBuilderDirectory);
            }
            catch (Exception)
            {
                Serilog.Log.Error($"Error on IssPackageBuilder: {typeof(T)}");
                throw;
            }

            // Deploy File
            var outputInno = OutputDirectory;
            var packageBuilderDirectory = GetMaxPathFolderOrTempFolder(PackageBuilderDirectory);
            var issFiles = Globbing.GlobFiles(packageBuilderDirectory, $"*{projectName}.iss");

            if (issFiles.IsEmpty())
                Serilog.Log.Error($"Not found any .iss file in {packageBuilderDirectory}");

            issFiles.ForEach(file =>
            {
                InnoSetupTasks.InnoSetup(config => config
                    .SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                    .SetScriptFile(file)
                    .SetOutputDir(outputInno));
            });

            // Sign outputInno
            SignFolder(outputInno);

            // Zip exe Files
            var exeFiles = Globbing.GlobFiles(outputInno, "**/*.exe");
            exeFiles.ForEach(file => ZipExtension.ZipFileCompact(file, projectNameVersion));

            if (exeFiles.IsEmpty())
                Serilog.Log.Error($"Not found any .exe file in {outputInno}");

            var message = string.Join(" | ", exeFiles.Select(e => e.Name));
            ReportSummary(_ => _.AddPair("File", message));

            if (outputInno != ReleaseDirectory)
            {
                Globbing.GlobFiles(outputInno, "**/*.zip")
                    .ForEach(file => AbsolutePathExtensions.CopyToDirectory(file, ReleaseDirectory));
            }

            var folder = Path.GetFileName(PackageBuilderDirectory);
            var releaseFileName = CreateReleaseFromDirectory(PackageBuilderDirectory, projectName, projectVersion, $".{folder}.zip");
            Serilog.Log.Information($"Release: {releaseFileName}");
        }

        if (releaseBundle)
        {
            var releaseFileName = CreateReleaseFromDirectory(BundleDirectory, projectName, projectVersion, ".bundle.zip", true);
            Serilog.Log.Information($"Release: {releaseFileName}");
            Serilog.Log.Information($"AppBundleTool -a \"{ReleaseDirectory / releaseFileName}\" -i");
        }
    }

    /// <summary>
    /// Create AddIns on each dll with the valid name
    /// </summary>
    /// <param name="project"></param>
    /// <param name="directory"></param>
    private IEnumerable<AbsolutePath> CreateAutoCADAddinOnProjectFiles(Project project, AbsolutePath directory)
    {
        var addInFiles = Globbing.GlobFiles(directory, $"**/*{project.Name}*.dll")
                        .Where(e => AutoCADExtension.HasAutoCADVersion(e));

        addInFiles.ForEach(file =>
        {
            var folder = file.Parent;
            SignFolder(folder, $"*{project.Name}*");
        });

        return addInFiles;
    }

    /// <summary>
    /// Check Folder if pass max path lenght return a copy with a temp folder
    /// </summary>
    /// <param name="packageBuilderDirectory"></param>
    /// <returns></returns>
    private AbsolutePath GetMaxPathFolderOrTempFolder(AbsolutePath packageBuilderDirectory)
    {
        const string TEMP_FOLDER = "PackageBuilder";
        const int MAX_PATH = 260;

        var temp = (AbsolutePath)Path.Combine(Path.GetTempPath(), TEMP_FOLDER);
        Serilog.Log.Information($"Path Max: {temp.ToString().Length} - {temp}");

        var file = packageBuilderDirectory;
        Serilog.Log.Information($"Path Max: {file.ToString().Length} - {Path.GetFileName(file)}");

        Globbing.GlobFiles(packageBuilderDirectory, "**/*")
            .ForEach(file =>
            {
                Serilog.Log.Information($"Path Max: {file.ToString().Length} - {Path.GetFileName(file)}");
            });

        var max = Globbing.GlobFiles(packageBuilderDirectory, "**/*").Max(file => file.ToString().Length);

        Serilog.Log.Information($"Path Max: {max}");
        if (max >= MAX_PATH)
        {
            if (temp.DirectoryExists()) temp.DeleteDirectory();
            AbsolutePathExtensions.Copy(packageBuilderDirectory, temp);
            var limit = max - file.ToString().Length + temp.ToString().Length;
            Serilog.Log.Information($"Path Max: {limit} - {temp}");
            return (AbsolutePath)temp;
        }

        return packageBuilderDirectory;
    }
}