using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazPackageBuilder : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// IExternalApplication Class 
        /// </summary>
        [Parameter]
        string Application => ValueInjectionUtility.TryGetValue(() => Application) ?? "App";

        /// <summary>
        /// Folder PackageBuilder 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "PackageBuilder";

        /// <summary>
        /// Project (default: <seealso cref="IHazPackageBuilderProject.GetPackageBuilderProject"/>)
        /// </summary>
        [Parameter]
        Project Project => ValueInjectionUtility.TryGetValue(() => Project) ?? GetPackageBuilderProject();

        AbsolutePath PackageBuilderDirectory => GetPackageBuilderDirectory(Project);
        public AbsolutePath GetPackageBuilderDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
