using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
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
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Output";

        /// <summary>
        /// Project (default: <seealso cref="IHazPackageBuilderProject.GetPackageBuilderProject"/>)
        /// </summary>
        [Parameter]
        public Project Project => ValueInjectionUtility.TryGetValue(() => Project) ?? GetPackageBuilderProject();

        /// <summary>
        /// OutputDirectory
        /// </summary>
        AbsolutePath OutputDirectory => GetOutputDirectory(Project);

        /// <summary>
        /// GetOutputDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetOutputDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
