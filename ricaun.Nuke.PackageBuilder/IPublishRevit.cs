using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    /// <summary>
    /// IPublishRevit
    /// </summary>
    public interface IPublishRevit : ICompile, IClean, ISign, IRelease, IRevitPackageBuilder, IGitRelease, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Build
        /// </summary>
        Target Build => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {

            });
    }
}
