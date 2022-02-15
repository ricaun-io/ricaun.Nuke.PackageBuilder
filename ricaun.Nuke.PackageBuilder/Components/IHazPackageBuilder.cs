using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPackageBuilder
    /// </summary>
    public interface IHazPackageBuilder : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder PackageBuilder 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "PackageBuilder";

        /// <summary>
        /// PackageBuilderDirectory
        /// </summary>
        AbsolutePath PackageBuilderDirectory => GetPackageBuilderDirectory(GetPackageBuilderProject());

        /// <summary>
        /// GetPackageBuilderDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetPackageBuilderDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
