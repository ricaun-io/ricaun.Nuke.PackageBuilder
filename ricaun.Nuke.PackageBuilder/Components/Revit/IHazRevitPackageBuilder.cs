using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazRevitPackageBuilder
    /// </summary>
    public interface IHazRevitPackageBuilder : IHazPackageBuilderProject, INukeBuild
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
        /// Add New Versions
        /// </summary>
        [Parameter]
        bool NewVersions => ValueInjectionUtility.TryGetValue<bool?>(() => NewVersions) ?? false;

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
    }
}
