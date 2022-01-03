using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack, ICompileExample, IRevitPackageBuilder
{
    private const string RevitProjectName = "RevitAddin.PackageBuilder.Example";
    string IHazPackageBuilderProject.Name => RevitProjectName;
    bool IHazPackageBuilderProject.ReleasePackageBuilder => true;
    string IHazPackageBuilder.Application => "Revit.App";
    string IHazExample.Name => RevitProjectName;
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
//    string IHazPackageBuilder.Application => "Revit.App";

//    IssConfiguration IHazInstallationFiles.IssConfiguration => new IssConfiguration()
//    {
//        IssLanguageLicences
//            = new[] {
//                new IssLanguageLicence() { Name="br", Licence = "License-br.rtf", MessagesFile = @"compiler:Languages\BrazilianPortuguese.isl"}
//            }
//    };
//    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
//}