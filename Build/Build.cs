using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

#if !PUBLISH_ONLY_REVIT
class Build : NukeBuild, IPublishPack, IRevitPackageBuilder, ITest, IPrePack
{
    string IHazInstallationFiles.InstallationFiles => "InstallationFiles";
    IssConfiguration IHazInstallationFiles.IssConfiguration => new IssConfiguration()
    {
        IssLanguageLicences = new[] {
            new IssLanguageLicence() { Name="br", Licence = "License-br.txt", MessagesFile = @"compiler:Languages\BrazilianPortuguese.isl"}
        }
    };
    string IHazPackageBuilderProject.Name => "Example";
    string IHazRevitPackageBuilder.Application => "Revit.App";
    string IHazRevitPackageBuilder.ApplicationType => "Application";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);

}
#else
/// <summary>
/// Create IPublishRevit
/// </summary>
class Build : NukeBuild, IPublishRevit
{
    private const string RevitProjectName = "RevitAddin.PackageBuilder.Example";
    bool IHazRelease.ReleaseNameVersion => true;
    string IHazMainProject.MainName => RevitProjectName;
    string IHazRevitPackageBuilder.Application => "Revit.App";

    IssConfiguration IHazInstallationFiles.IssConfiguration => new IssConfiguration()
    {
        Title = "Example",
        IssLanguageLicences
            = new[] {
                new IssLanguageLicence() { Name="br", Licence = "License-br.txt", MessagesFile = @"compiler:Languages\BrazilianPortuguese.isl"}
            }
    };
    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
}
#endif