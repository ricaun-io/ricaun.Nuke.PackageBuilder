using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using ricaun.Nuke;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack, ICompileExample, IRevitPackageBuilder
{
    string IHazInput.Folder => "Content";
    string IHazPackageBuilderProject.Name => "RevitAddin.PackageBuilder.Example";

    string IHazExample.Name => "RevitAddin.PackageBuilder.Example";
    string IHazExample.Folder => "Content";

    string IHazContent.Folder => "Release";
    string IHazRelease.Folder => "ReleasePack";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}