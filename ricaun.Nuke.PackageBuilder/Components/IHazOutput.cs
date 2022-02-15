using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazOutput
    /// </summary>
    public interface IHazOutput : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Output 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Output";

        /// <summary>
        /// OutputDirectory
        /// </summary>
        AbsolutePath OutputDirectory => GetOutputDirectory(GetPackageBuilderProject());

        /// <summary>
        /// GetOutputDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetOutputDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
