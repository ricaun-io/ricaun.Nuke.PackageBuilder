using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using System;
using System.IO;
using System.IO.Compression;

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
        string InstallationFiles => TryGetValue(() => InstallationFiles) ?? "InstallationFiles";

        /// <summary>
        /// Configuration
        /// </summary>
        [Parameter]
        IssConfiguration IssConfiguration => TryGetValue(() => IssConfiguration) ?? new IssConfiguration();

        /// <summary>
        /// InstallationFilesDirectory
        /// </summary>
        AbsolutePath InstallationFilesDirectory => GetInstallationFilesDirectory(GetPackageBuilderProject());

        /// <summary>
        /// GetInstallationFilesDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetInstallationFilesDirectory(Project project) => project.Directory / InstallationFiles;

        /// <summary>
        /// CopyInstallationFilesTo
        /// </summary>
        /// <param name="packageBuilderDirectory"></param>
        public void CopyInstallationFilesTo(AbsolutePath packageBuilderDirectory)
        {
            Serilog.Log.Information($"InstallationFiles: {InstallationFiles}");
            DownloadFilesAndUnzip(InstallationFiles, packageBuilderDirectory);

            PathConstruction.GlobFiles(InstallationFilesDirectory, "*")
                .ForEach(file => FileSystemTasks.CopyFileToDirectory(file, packageBuilderDirectory));
        }

        /// <summary>
        /// Download Files from url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downloadFolder"></param>
        /// <returns></returns>
        private bool DownloadFilesAndUnzip(string url, string downloadFolder)
        {
            try
            {
                var fileName = Path.GetFileName(url);
                var file = Path.Combine(downloadFolder, fileName);

                HttpClientExtension.DownloadFile(url, file);

                if (Path.GetExtension(file).EndsWith("zip"))
                {
                    ZipFile.ExtractToDirectory(file, Path.GetDirectoryName(file));
                    File.Delete(file);
                }
                Serilog.Log.Information($"DownloadFiles: {fileName}");
                return true;
            }
            catch { }
            return false;
        }
    }
}
