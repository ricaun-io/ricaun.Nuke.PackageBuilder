using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.InnoSetup;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System.IO;

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

            ContentsDirectory = ContentsDirectory / "567890456789"; // <<<<<<<<<<<<<<<<<<<<<<<<< 1234 \\?\

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


            string result = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Serilog.Log.Warning($"{result.ToString().Length} - {result}");

            string temp = Path.GetTempFileName();
            Serilog.Log.Warning($"{temp.ToString().Length} - {temp}");

            var file = PackageBuilderDirectory;
            Serilog.Log.Warning($"{file.ToString().Length} - {Path.GetFileName(file)}");

            PathConstruction.GlobFiles(PackageBuilderDirectory, "**/*")
                .ForEach(file =>
                {
                    Serilog.Log.Warning($"{file.ToString().Length} - {Path.GetFileName(file)}");
                });

            // Deploy File
            var outputInno = OutputDirectory;
            var issFiles = PathConstruction.GlobFiles(PackageBuilderDirectory, $"*{project.Name}.iss");
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
    }
}
