using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke;

/// <summary>
/// IPublishNavisworks
/// </summary>
public interface IPublishNavisworks : ICompile, IClean, ISign, IRelease, INavisworksPackageBuilder, IGitRelease, IHazSolution, INukeBuild
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