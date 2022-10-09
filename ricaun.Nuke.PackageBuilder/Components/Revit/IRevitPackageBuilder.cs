using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.InnoSetup;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IRevitPackageBuilder
    /// </summary>
    public interface IRevitPackageBuilder : IHazRevitPackageBuilder, IHazPackageBuilderProject, IHazInstallationFiles, IRelease, ISign, IHazPackageBuilder, IHazInput, IHazOutput, INukeBuild
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

            var projectNameVersion = GetReleaseFileNameVersion(projectName, projectVersion);

            var bundleName = $"{projectName}.bundle";
            var BundleDirectory = PackageBuilderDirectory / bundleName;
            var ContentsDirectory = BundleDirectory / "Contents";

            if (ProjectNameFolder)
                ContentsDirectory = ContentsDirectory / projectName;

            if (ProjectVersionFolder)
                ContentsDirectory = ContentsDirectory / projectVersion;

            FileSystemTasks.CopyDirectoryRecursively(InputDirectory, ContentsDirectory);

            CreateRevitAddinOnProjectFiles(project, ContentsDirectory);

            // CopyInstallationFiles If Exists
            CopyInstallationFilesTo(PackageBuilderDirectory);

            new RevitContentsBuilder(project, BundleDirectory, NewVersions)
                .Build(BundleDirectory / "PackageContents.xml");

            new IssRevitBuilder(project, PackageBuilderDirectory, IssConfiguration)
                .CreateFile(PackageBuilderDirectory);

            // Deploy File
            var outputInno = OutputDirectory;
            var packageBuilderDirectory = GetMaxPathFolderOrTempFolder(PackageBuilderDirectory);
            var issFiles = PathConstruction.GlobFiles(packageBuilderDirectory, $"*{projectName}.iss");
            issFiles.ForEach(file =>
            {
                InnoSetupTasks.InnoSetup(config => config
                    .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                    .SetScriptFile(file)
                    .SetOutputDir(outputInno));
            });

            // Sign outputInno
            SignFolder(outputInno);

            // Zip exe Files
            var exeFiles = PathConstruction.GlobFiles(outputInno, "**/*.exe");
            exeFiles.ForEach(file => ZipExtension.ZipFileCompact(file, projectNameVersion));

            if (outputInno != ReleaseDirectory)
            {
                PathConstruction.GlobFiles(outputInno, "**/*.zip")
                    .ForEach(file => FileSystemTasks.CopyFileToDirectory(file, ReleaseDirectory));
            }

            if (releasePackageBuilder)
            {
                var folder = Path.GetFileName(PackageBuilderDirectory);
                var releaseFileName = CreateReleaseFromDirectory(PackageBuilderDirectory, projectName, projectVersion, $".{folder}.zip");
                Serilog.Log.Information($"Release: {releaseFileName}");
            }

            if (releaseBundle)
            {
                var releaseFileName = CreateReleaseFromDirectory(BundleDirectory, projectName, projectVersion, ".bundle.zip", true);
                Serilog.Log.Information($"Release: {releaseFileName}");
            }
        }

        /// <summary>
        /// Create AddIns on each dll with the valid name
        /// </summary>
        /// <param name="project"></param>
        /// <param name="directory"></param>
        private void CreateRevitAddinOnProjectFiles(Project project, AbsolutePath directory)
        {
            var addInFiles = PathConstruction.GlobFiles(directory, $"**/*{project.Name}*.dll")
                            .Where(e => RevitExtension.HasRevitVersion(e));

            addInFiles.ForEach(file =>
            {
                SignFolder(file);
                new RevitProjectAddInsBuilder(project, file, Application, ApplicationType, VendorId, VendorDescription)
                    .Build(file);
            });
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

            PathConstruction.GlobFiles(packageBuilderDirectory, "**/*")
                .ForEach(file =>
                {
                    Serilog.Log.Information($"Path Max: {file.ToString().Length} - {Path.GetFileName(file)}");
                });

            var max = PathConstruction.GlobFiles(packageBuilderDirectory, "**/*").Max(file => file.ToString().Length);

            Serilog.Log.Information($"Path Max: {max}");
            if (max >= MAX_PATH)
            {
                if (FileSystemTasks.DirectoryExists(temp)) FileSystemTasks.DeleteDirectory(temp);
                FileSystemTasks.CopyDirectoryRecursively(packageBuilderDirectory, temp);
                var limit = max - file.ToString().Length + temp.ToString().Length;
                Serilog.Log.Information($"Path Max: {limit} - {temp}");
                return (AbsolutePath)temp;
            }

            return packageBuilderDirectory;
        }
    }
}
