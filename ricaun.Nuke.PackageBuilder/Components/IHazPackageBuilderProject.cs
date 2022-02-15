using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPackageBuilderProject
    /// </summary>
    public interface IHazPackageBuilderProject : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// PackageBuilder Project Name
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? MainName;

        /// <summary>
        /// ReleasePackageBuilder (default: false)
        /// </summary>
        [Parameter]
        bool ReleasePackageBuilder => TryGetValue<bool?>(() => ReleasePackageBuilder) ?? false;

        /// <summary>
        /// GetPackageBuilderProject
        /// </summary>
        /// <returns></returns>
        public Project GetPackageBuilderProject() => Solution.GetOtherProject(Name);
    }
}
