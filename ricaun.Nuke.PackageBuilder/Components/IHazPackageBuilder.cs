using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazPackageBuilder
    /// </summary>
    public interface IHazPackageBuilder : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// IExternalApplication Class 
        /// </summary>
        [Parameter]
        string Application => ValueInjectionUtility.TryGetValue(() => Application) ?? "App";

        /// <summary>
        /// GetApplication
        /// </summary>
        /// <returns></returns>
        public string GetApplication() => Application;

        /// <summary>
        /// VendorId
        /// </summary>
        [Parameter]
        string VendorId => ValueInjectionUtility.TryGetValue(() => VendorId) ?? GetPackageBuilderProject().GetCompany();

        /// <summary>
        /// VendorDescription
        /// </summary>
        [Parameter]
        string VendorDescription => ValueInjectionUtility.TryGetValue(() => VendorDescription) ?? VendorId;

        /// <summary>
        /// Folder PackageBuilder 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "PackageBuilder";

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
