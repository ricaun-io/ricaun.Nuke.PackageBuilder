using Nuke.Common;
using ricaun.Nuke.Components;

namespace ricaun.Nuke
{
    public interface IPublishRevit : ICompile, IClean, ISign, IRelease, IRevitPackageBuilder, IGitRelease, IHazSolution, INukeBuild
    {
        Target Build => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {

            });
    }
}
