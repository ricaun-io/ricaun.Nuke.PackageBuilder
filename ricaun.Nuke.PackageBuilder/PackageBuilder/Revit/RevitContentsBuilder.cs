using Autodesk.PackageBuilder;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;
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
        /// Default plus year if lastVersionRevit is true
        /// </summary>
        private const int LAST_VERSION_PLUS_YEAR = 1;

        /// <summary>
        /// RevitContentsBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="bundleDirectory"></param>
        /// <param name="middleVersionRevit"></param>
        /// <param name="lastVersionRevit"></param>
        public RevitContentsBuilder(Project project, AbsolutePath bundleDirectory,
            bool middleVersionRevit, bool lastVersionRevit)
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

            var addinFiles = Globbing.GlobFiles(bundleDirectory, $"**/*{project.Name}*.addin");

            //var lastVersion = 0;
            //foreach (var addinFile in addinFiles)
            //    lastVersion = AddRevitComponentsByFileVersion(project, addinFile, bundleDirectory);

            //if (lastVersionRevit)
            //    while (lastVersion <= DateTime.Now.Year + LAST_VERSION_PLUS_YEAR)
            //    {
            //        lastVersion = AddRevitComponentsByFileVersion(project, addinFiles.Last(), bundleDirectory, lastVersion + 1);
            //    }

            var fileVersion = new Dictionary<int, AbsolutePath>();

            var lastVersion = 0;
            foreach (var addinFile in addinFiles)
            {
                lastVersion = AddRevitComponentsByFileVersion(project, addinFile, bundleDirectory);
                fileVersion[lastVersion] = addinFile;
            }

            if (middleVersionRevit)
            {
                Serilog.Log.Information($"Components Middle Version");
                var versions = fileVersion.Keys.ToList();
                var lowVersion = versions.Min();
                for (int v = versions.Min(); v < versions.Max(); v++)
                {
                    if (versions.Contains(v))
                    {
                        lowVersion = v;
                        continue;
                    }
                    AddRevitComponentsByFileVersion(project, fileVersion[lowVersion], bundleDirectory, v);
                }
            }

            if (lastVersionRevit)
            {
                Serilog.Log.Information($"Components Last Version");
                while (lastVersion <= DateTime.Now.Year + LAST_VERSION_PLUS_YEAR)
                {
                    lastVersion = AddRevitComponentsByFileVersion(project, addinFiles.Last(), bundleDirectory, lastVersion + 1);
                }
            }

        }

        private int AddRevitComponentsByFileVersion(Project project, AbsolutePath addinFile, AbsolutePath bundleDirectory, int version = 0)
        {
            var appName = project.Name;

            var moduleName = ((string)addinFile).Replace(bundleDirectory, ".");

            var folder = Path.GetDirectoryName(addinFile);
            var dll = Globbing.GlobFiles(folder, $"*{project.Name}*.dll")
                .Where(e => RevitExtension.HasRevitVersion(e))
                .FirstOrDefault();

            if (dll == null)
            {
                Serilog.Log.Warning($"File on Project {project.Name} does not have Revit Version");
                return version;
            }

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