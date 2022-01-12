using Autodesk.PackageBuilder;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// RevitContentsBuilder
    /// </summary>
    public class RevitContentsBuilder : PackageContentsBuilder
    {
        /// <summary>
        /// RevitContentsBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="bundleDirectory"></param>
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

            var addinFiles = PathConstruction.GlobFiles(bundleDirectory, $"**/*{project.Name}*.addin");

            foreach (var addinFile in addinFiles)
            {
                var moduleName = ((string)addinFile).Replace(bundleDirectory, ".");
                if (moduleName.StartsWith("."))
                {
                    AddRevitComponentsByFileVersion(project, addinFile, appName, moduleName);
                }
            }
        }

        private void AddRevitComponentsByFileVersion(Project project, AbsolutePath addinFile, string appName, string moduleName)
        {
            var folder = Path.GetDirectoryName(addinFile);
            var dll = PathConstruction.GlobFiles(folder, $"*{project.Name}*.dll").FirstOrDefault();
            var version = RevitExtension.GetRevitVersion(dll);

            Components
                .CreateEntry($"{AutodeskProducts.Revit} {version}")
                .AppName(appName)
                .ModuleName(moduleName)
                .RevitPlatform(version);

            Logger.Normal($"Component Revit {version}: {Path.GetFileName(dll)}");
        }
    }
}