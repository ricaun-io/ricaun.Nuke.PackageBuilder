using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

#if !PUBLISH_ONLY_REVIT
[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack, IRevitPackageBuilder
{
    string IHazPackageBuilderProject.Name => "Example";
    string IHazRevitPackageBuilder.Application => "Revit.App";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
#else
/// <summary>
/// Create IPublishRevit
/// </summary>
[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishRevit
{
    private const string RevitProjectName = "RevitAddin.PackageBuilder.Example";
    string IHazMainProject.MainName => RevitProjectName;
    string IHazRevitPackageBuilder.Application => "Revit.App";

    IssConfiguration IHazInstallationFiles.IssConfiguration => new IssConfiguration()
    {
        IssLanguageLicences
            = new[] {
                new IssLanguageLicence() { Name="br", Licence = "License-br.txt", MessagesFile = @"compiler:Languages\BrazilianPortuguese.isl"}
            }
    };
    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
}
#endif