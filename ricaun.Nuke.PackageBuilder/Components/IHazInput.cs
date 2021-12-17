using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazInput : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Input 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// Project (default: <seealso cref="IHazPackageBuilderProject.GetPackageBuilderProject"/>)
        /// </summary>
        [Parameter]
        Project Project => ValueInjectionUtility.TryGetValue(() => Project) ?? GetPackageBuilderProject();
        AbsolutePath InputDirectory => GetInputDirectory(Project);
        public AbsolutePath GetInputDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
