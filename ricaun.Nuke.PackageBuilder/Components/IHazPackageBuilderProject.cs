using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazPackageBuilderProject : IHazSolution, INukeBuild
    {
        /// <summary>
        /// PackageBuilder Project Name
        /// </summary>
        [Parameter]
        string Name => ValueInjectionUtility.TryGetValue(() => Name) ?? Solution.Name;
        public Project GetPackageBuilderProject() => Solution.GetOtherProject(Name);
    }
}
