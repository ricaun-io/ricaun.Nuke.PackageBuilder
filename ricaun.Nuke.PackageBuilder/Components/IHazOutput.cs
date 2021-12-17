using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
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
        AbsolutePath OutputDirectory => GetOutputDirectory(Project);
        public AbsolutePath GetOutputDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
