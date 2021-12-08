using Autodesk.PackageBuilder;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;

namespace ricaun.Nuke.Components
{
    public class RevitContentsBuilder : PackageContentsBuilder
    {
        public RevitContentsBuilder(Project project, AbsolutePath bundleDirectory)
        {
            var appName = project.Name;

            ApplicationPackage
                .Create()
                .RevitApplication()
                .Name(appName)
                .ProductCode(project.GetAppId())
                .AppVersion(project.GetVersion());

            CompanyDetails
                .Create(project.GetCompany());

            var files = PathConstruction.GlobFiles(bundleDirectory, $"**/*{project.Name}*.addin");

            foreach (var file in files)
            {
                var moduleName = ((string)file).Replace(bundleDirectory, ".");
                if (moduleName.StartsWith("."))
                {
                    var version = Path.GetFileName(Path.GetDirectoryName(file));
                    var i = int.Parse(version);
                    Components
                        .CreateEntry($"{AutodeskProducts.Revit} {i}")
                        .AppName(appName)
                        .ModuleName(moduleName)
                        .RevitPlatform(i);

                }
            }
        }
    }
}