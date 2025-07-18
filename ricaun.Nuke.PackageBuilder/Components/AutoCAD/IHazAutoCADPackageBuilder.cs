using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazAutoCADPackageBuilder
    /// </summary>
    public interface IHazAutoCADPackageBuilder : IHazPackageBuilderProject, INukeBuild
    {
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
    }
}
