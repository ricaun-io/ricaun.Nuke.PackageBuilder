using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishRevit
{
    string IHazPackageBuilder.Application => "Revit.App";
    string IHazPackageBuilder.Folder => "PackageBuilder";
    string IHazOutput.Folder => "Output";
    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
}
