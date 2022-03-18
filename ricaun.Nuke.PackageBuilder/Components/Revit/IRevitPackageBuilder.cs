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
            var fileName = $"{project.Name}";
            var bundleName = $"{fileName}.bundle";
            var BundleDirectory = PackageBuilderDirectory / bundleName;
            var ContentsDirectory = BundleDirectory / "Contents";

            if (ProjectNameFolder)
                ContentsDirectory = ContentsDirectory / project.Name;

            if (ProjectVersionFolder)
                ContentsDirectory = ContentsDirectory / project.GetInformationalVersion();

            FileSystemTasks.CopyDirectoryRecursively(InputDirectory, ContentsDirectory);

            var addInFiles = PathConstruction.GlobFiles(ContentsDirectory, $"**/*{project.Name}*.dll");
            addInFiles.ForEach(file =>
            {
                new RevitProjectAddInsBuilder(project, file, Application, VendorId, VendorDescription).Build(file);
            });

            // CopyInstallationFiles If Exists
            CopyInstallationFilesTo(PackageBuilderDirectory);

            new RevitContentsBuilder(project, BundleDirectory, NewVersions)
                .Build(BundleDirectory / "PackageContents.xml");

            new IssRevitBuilder(project, PackageBuilderDirectory, IssConfiguration)
                .CreateFile(PackageBuilderDirectory);

            // Deploy File
            var outputInno = OutputDirectory;
            var packageBuilderDirectory = GetMaxPathFolderOrTempFolder(PackageBuilderDirectory);
            var issFiles = PathConstruction.GlobFiles(packageBuilderDirectory, $"*{project.Name}.iss");
            issFiles.ForEach(file =>
            {
                InnoSetupTasks.InnoSetup(config => config
                    .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                    .SetScriptFile(file)
                    .SetOutputDir(outputInno));
            });

            // Sign outputInno
            SignFolder(outputInno);

            var exeFiles = PathConstruction.GlobFiles(outputInno, "**/*.exe");
            exeFiles.ForEach(file => ZipExtension.ZipFileCompact(file));

            if (outputInno != ReleaseDirectory)
            {
                PathConstruction.GlobFiles(outputInno, "**/*.zip")
                    .ForEach(file => FileSystemTasks.CopyFileToDirectory(file, ReleaseDirectory));
            }

            if (releasePackageBuilder)
            {
                var folder = Path.GetFileName(PackageBuilderDirectory);
                ZipExtension.CreateFromDirectory(PackageBuilderDirectory, ReleaseDirectory / $"{project.Name} {folder}.zip");
            }

            if (releaseBundle)
            {
                ZipExtension.CreateFromDirectory(BundleDirectory, ReleaseDirectory / $"{bundleName}.zip");
            }
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
