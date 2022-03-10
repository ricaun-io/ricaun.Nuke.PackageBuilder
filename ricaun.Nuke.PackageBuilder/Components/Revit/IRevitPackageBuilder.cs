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
                CreatePackageBuilder(GetPackageBuilderProject(), ReleasePackageBuilder, ReleaseBundle);
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

            //ContentsDirectory = ContentsDirectory / "12345678904567890" / "13a2s213d132as132312ads" / "12345678901234567890"; // <<<<<<<<<<<<<<<<<<<<<<<<< 1234 \\?\

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
            var packageBuilderDirectory = GetFolderWithMaxPath(PackageBuilderDirectory);
            var issFiles = PathConstruction.GlobFiles(packageBuilderDirectory, $"*{project.Name}.iss");
            issFiles.ForEach(file =>
            {
                InnoSetupTasks.InnoSetup(config => config
                    .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                    .SetScriptFile(file)
                    .SetOutputDir(outputInno));
            });

            // Sign Project
            SignProject(project);

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

        private AbsolutePath GetFolderWithMaxPath(AbsolutePath packageBuilderDirectory)
        {
            var temp = (AbsolutePath)Path.Combine(Path.GetTempPath(), "PackageBuilder");
            Serilog.Log.Information($"Path Max: {temp.ToString().Length} - {temp}");

            var file = packageBuilderDirectory;
            Serilog.Log.Information($"Path Max: {file.ToString().Length} - {Path.GetFileName(file)}");

            PathConstruction.GlobFiles(packageBuilderDirectory, "**/*")
                .ForEach(file =>
                {
                    Serilog.Log.Information($"Path Max: {file.ToString().Length} - {Path.GetFileName(file)}");
                });

            var max = PathConstruction.GlobFiles(packageBuilderDirectory, "**/*").Max(file => file.ToString().Length);

            Serilog.Log.Warning($"Path Max: {max}");
            if (max >= 260)
            {
                if (FileSystemTasks.DirectoryExists(temp)) FileSystemTasks.DeleteDirectory(temp);
                FileSystemTasks.CopyDirectoryRecursively(packageBuilderDirectory, temp);
                var limit = max - file.ToString().Length + temp.ToString().Length;
                Serilog.Log.Warning($"Path Max: {limit} - {temp}");
                return (AbsolutePath)temp;
            }

            return packageBuilderDirectory;
        }
    }
}
