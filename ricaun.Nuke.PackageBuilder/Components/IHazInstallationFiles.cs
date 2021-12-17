using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.ValueInjection;

namespace ricaun.Nuke.Components
{
    public interface IHazInstallationFiles : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder InstallationFiles 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "InstallationFiles";

        /// <summary>
        /// Project (default: <seealso cref="IHazPackageBuilderProject.GetPackageBuilderProject"/>)
        /// </summary>
        [Parameter]
        Project Project => ValueInjectionUtility.TryGetValue(() => Project) ?? GetPackageBuilderProject();
        AbsolutePath InstallationFilesDirectory => GetInstallationFilesDirectory(Project);
        public AbsolutePath GetInstallationFilesDirectory(Project project) => project.Directory / Folder;
        public void CopyInstallationFilesTo(AbsolutePath packageBuilderDirectory)
        {
            PathConstruction.GlobFiles(InstallationFilesDirectory, "*")
                .ForEach(file => FileSystemTasks.CopyFileToDirectory(file, packageBuilderDirectory));
        }
    }
}
