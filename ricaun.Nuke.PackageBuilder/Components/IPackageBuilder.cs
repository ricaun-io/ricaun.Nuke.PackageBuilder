using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.InnoSetup;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.ValueInjection;
using Octokit;
using ricaun.Nuke.Extensions;
using System;
using System.IO;

namespace ricaun.Nuke.Components
{
    public interface IPackageBuilder : IRelease, IHazContent, ISign, IHazPackageBuilder, IHazOutput, INukeBuild
    {
        Target PackageBuilder => _ => _
            .TriggeredBy(Sign)
            .Before(Release)
            .Executes(() =>
            {
                var project = Solution.GetMainProject();
                var fileName = $"{project.Name}";
                var bundleName = $"{fileName}.bundle";
                var BundleDirectory = PackageBuilderDirectory / bundleName;
                var ContentsDirectory = BundleDirectory / "Contents";

                FileSystemTasks.CopyDirectoryRecursively(ContentDirectory, ContentsDirectory);

                var addInFiles = PathConstruction.GlobFiles(ContentsDirectory, $"**/*{project.Name}*.dll");
                addInFiles.ForEach(file =>
                {
                    new ProjectAddInsBuilder(project, file, Application).Build(file);
                });

                new RevitContentsBuilder(project, BundleDirectory)
                    .Build(BundleDirectory / "PackageContents.xml");

                new IssRevitBuilder(project)
                    .CreateFile(PackageBuilderDirectory);

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
            });
    }
}
