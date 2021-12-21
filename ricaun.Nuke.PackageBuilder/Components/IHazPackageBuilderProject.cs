using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
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
        string Name => ValueInjectionUtility.TryGetValue(() => Name) ?? MainName;

        /// <summary>
        /// PackageBuilder Project Name
        /// </summary>
        [Parameter]
        bool ReleasePackageBuilder => ValueInjectionUtility.TryGetValue<bool?>(() => ReleasePackageBuilder) ?? false;

        /// <summary>
        /// GetPackageBuilderProject
        /// </summary>
        /// <returns></returns>
        public Project GetPackageBuilderProject() => Solution.GetOtherProject(Name);
    }
}
