using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack, ICompileExample, IRevitPackageBuilder
{
    string IHazPackageBuilderProject.Name => "Example";
    bool IHazPackageBuilderProject.ReleasePackageBuilder => true;
    bool IHazPackageBuilderProject.ReleaseBundle => true;
    bool IHazPackageBuilderProject.ProjectVersionFolder => true;
    bool IHazPackageBuilderProject.ProjectNameFolder => true;
    string IHazRevitPackageBuilder.Application => "Revit.App";
    bool IHazRevitPackageBuilder.NewVersions => true;
    bool IHazExample.ReleaseExample => false;
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}

///// <summary>
///// Create RevitProjectName
///// </summary>
//[CheckBuildProjectConfigurations]
//class Build : NukeBuild, IPublishRevit
//{
//    private const string RevitProjectName = "RevitAddin.PackageBuilder.Example";
//    string IHazMainProject.MainName => RevitProjectName;
//    bool IHazRevitPackageBuilder.NewVersions => true;
//    string IHazRevitPackageBuilder.Application => "Revit.App";

//    IssConfiguration IHazInstallationFiles.IssConfiguration => new IssConfiguration()
//    {
//        IssLanguageLicences
//            = new[] {
//                new IssLanguageLicence() { Name="br", Licence = "License-br.txt", MessagesFile = @"compiler:Languages\BrazilianPortuguese.isl"}
//            }
//    };
//    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
//}