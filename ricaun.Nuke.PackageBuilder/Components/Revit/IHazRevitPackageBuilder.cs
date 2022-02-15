﻿using Nuke.Common;
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
        /// IExternalApplication Class 
        /// </summary>
        [Parameter]
        string Application => TryGetValue(() => Application) ?? "App";

        /// <summary>
        /// GetApplication
        /// </summary>
        /// <returns></returns>
        public string GetApplication() => Application;

        /// <summary>
        /// Add New Versions
        /// </summary>
        [Parameter]
        bool NewVersions => TryGetValue<bool?>(() => NewVersions) ?? false;

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
    }
}
