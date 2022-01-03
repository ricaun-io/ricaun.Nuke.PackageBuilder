using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.ValueInjection;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazInstallationFiles
    /// </summary>
    public interface IHazInstallationFiles : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder InstallationFiles 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "InstallationFiles";

        /// <summary>
        /// Configuration
        /// </summary>
        [Parameter]
        IssConfiguration IssConfiguration => ValueInjectionUtility.TryGetValue(() => IssConfiguration) ?? new IssConfiguration();

        /// <summary>
        /// InstallationFilesDirectory
        /// </summary>
        AbsolutePath InstallationFilesDirectory => GetInstallationFilesDirectory(GetPackageBuilderProject());

        /// <summary>
        /// GetInstallationFilesDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetInstallationFilesDirectory(Project project) => project.Directory / Folder;

        /// <summary>
        /// CopyInstallationFilesTo
        /// </summary>
        /// <param name="packageBuilderDirectory"></param>
        public void CopyInstallationFilesTo(AbsolutePath packageBuilderDirectory)
        {
            PathConstruction.GlobFiles(InstallationFilesDirectory, "*")
                .ForEach(file => FileSystemTasks.CopyFileToDirectory(file, packageBuilderDirectory));
        }
    }
}
