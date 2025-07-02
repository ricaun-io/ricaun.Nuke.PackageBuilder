using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke;

/// <summary>
/// IPublishAutoCAD
/// </summary>
public interface IPublishAutoCAD : ICompile, IClean, ISign, IRelease, IAutoCADPackageBuilder, IGitRelease, IHazSolution, INukeBuild
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