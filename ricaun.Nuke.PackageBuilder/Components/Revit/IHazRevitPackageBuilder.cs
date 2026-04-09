using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazRevitPackageBuilder
    /// </summary>
    public interface IHazRevitPackageBuilder : IHazPackageBuilderProject, INukeBuild
    {
        /// <summary>
        /// IExternalApplication Class (default: "App")
        /// </summary>
        [Parameter]
        string Application => TryGetValue(() => Application) ?? "App";

        /// <summary>
        /// Application Type (default: "Application")
        /// </summary>
        [Parameter]
        string ApplicationType => TryGetValue(() => ApplicationType) ?? "Application";

        /// <summary>
        /// GetApplication
        /// </summary>
        /// <returns></returns>
        public string GetApplication() => Application;

        /// <summary>
        /// Add Middle Versions (default: true)
        /// </summary>
        [Parameter]
        bool MiddleVersions => TryGetValue<bool?>(() => MiddleVersions) ?? true;

        /// <summary>
        /// Add New Versions (default: true)
        /// </summary>
        [Parameter]
        bool NewVersions => TryGetValue<bool?>(() => NewVersions) ?? true;

        /// <summary>
        /// VendorId
        /// </summary>
        [Parameter]
        string VendorId => TryGetValue(() => VendorId) ?? GetPackageBuilderProject().GetCompany();

        /// <summary>
        /// VendorDescription
        /// </summary>
        [Parameter]
        string VendorDescription => TryGetValue(() => VendorDescription) ?? VendorId;

        /// <summary>
        /// RevitContextIsolation (Support in Revit 2027+) (Default: false)
        /// </summary>
        /// <remarks>
        /// When enabled the `ManifestSettings.UseRevitContext` is set to false and the addin is loaded in a isolated context.
        /// </remarks>
        [Parameter]
        bool RevitContextIsolation => TryGetValue<bool?>(() => RevitContextIsolation) ?? false;

        /// <summary>
        /// RevitContextName (Support in Revit 2027+)
        /// </summary>
        /// <remarks>
        /// When used the `ManifestSettings.UseRevitContext` is set to false and the addin is loaded in a isolated context with a custom `ManifestSettings.ContextName`.
        /// </remarks>
        [Parameter]
        string RevitContextName => TryGetValue(() => RevitContextName) ?? null;

        /// <summary>
        /// RevitContextVersion (Default 2027) (Lowest version to add `ManifestSettings.UseRevitContext` and `ManifestSettings.ContextName`)
        /// </summary>
        [Parameter]
        int RevitContextVersion => TryGetValue<int?>(() => RevitContextVersion) ?? 2027;
    }
}
