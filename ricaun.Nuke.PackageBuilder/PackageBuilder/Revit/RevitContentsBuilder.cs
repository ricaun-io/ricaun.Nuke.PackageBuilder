using Autodesk.PackageBuilder;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System;
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
        /// <param name="lastVersionRevit"></param>
        public RevitContentsBuilder(Project project, AbsolutePath bundleDirectory, bool lastVersionRevit = false)
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

            var lastVersion = 0;
            foreach (var addinFile in addinFiles)
                lastVersion = AddRevitComponentsByFileVersion(project, addinFile, bundleDirectory);

            if (lastVersionRevit)
                while (lastVersion <= DateTime.Now.Year)
                {
                    lastVersion = AddRevitComponentsByFileVersion(project, addinFiles.Last(), bundleDirectory, lastVersion + 1);
                }

        }

        private int AddRevitComponentsByFileVersion(Project project, AbsolutePath addinFile, AbsolutePath bundleDirectory, int version = 0)
        {
            var appName = project.Name;

            var moduleName = ((string)addinFile).Replace(bundleDirectory, ".");

            var folder = Path.GetDirectoryName(addinFile);
            var dll = PathConstruction.GlobFiles(folder, $"*{project.Name}*.dll").FirstOrDefault();

            if (version == 0)
                version = RevitExtension.GetRevitVersion(dll);

            Components
                .CreateEntry($"{AutodeskProducts.Revit} {version}")
                .AppName(appName)
                .ModuleName(moduleName)
                .RevitPlatform(version);

            Serilog.Log.Information($"Component Revit {version}: {Path.GetFileName(dll)}");

            return version;
        }
    }
}