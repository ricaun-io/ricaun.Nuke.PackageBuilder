using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

using ricaun.Nuke.Extensions;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPackageBuilderProject
    /// </summary>
    public interface IHazPackageBuilderProject : IHazMainProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// PackageBuilder Project Name or EndWith Name
        /// </summary>
        [Parameter]
        string Name => TryGetValue(() => Name) ?? MainName;

        /// <summary>
        /// ReleasePackageBuilder (default: true)
        /// </summary>
        [Parameter]
        bool ReleasePackageBuilder => TryGetValue<bool?>(() => ReleasePackageBuilder) ?? true;

        /// <summary>
        /// ReleaseBundle (default: true)
        /// </summary>
        [Parameter]
        bool ReleaseBundle => TryGetValue<bool?>(() => ReleaseBundle) ?? true;

        /// <summary>
        /// Add ProjectNameFolder on the Contents (default: true)
        /// </summary>
        [Parameter]
        bool ProjectNameFolder => TryGetValue<bool?>(() => ProjectNameFolder) ?? true;

        /// <summary>
        /// Add ProjectVersionFolder on the Contents (default: true)
        /// </summary>
        [Parameter]
        bool ProjectVersionFolder => TryGetValue<bool?>(() => ProjectVersionFolder) ?? true;

        /// <summary>
        /// Add ProjectRemoveTargetFrameworkFolder on the Contents (default: true)
        /// </summary>
        [Parameter]
        bool ProjectRemoveTargetFrameworkFolder => TryGetValue<bool?>(() => ProjectRemoveTargetFrameworkFolder) ?? true;

        /// <summary>
        /// GetPackageBuilderProject by the Name
        /// </summary>
        /// <returns></returns>
        public Project GetPackageBuilderProject()
        {
            if (Solution.GetOtherProject(Name) is Project project)
                return project;
            return Solution.GetOtherProjects(Name).First();
        }
    }
}
