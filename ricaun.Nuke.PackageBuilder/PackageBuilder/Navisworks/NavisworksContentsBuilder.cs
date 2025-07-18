using Autodesk.PackageBuilder;
using Autodesk.PackageBuilder.Extensible.Navisworks;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components;

/// <summary>
/// Provides functionality to build Navisworks package contents for deployment.
/// </summary>
public class NavisworksContentsBuilder : PackageContentsBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavisworksContentsBuilder"/> class.
    /// Builds the Navisworks package contents based on the provided project, bundle directory, and add-in files.
    /// </summary>
    /// <param name="project">The project containing metadata for the package.</param>
    /// <param name="bundleDirectory">The directory where the bundle is located.</param>
    /// <param name="addinFiles">A collection of add-in files to include in the package.</param>
    public NavisworksContentsBuilder(Project project, AbsolutePath bundleDirectory,
        IEnumerable<AbsolutePath> addinFiles)
    {
        var appName = project.Name;

        ApplicationPackage
            .Create()
            .NavisworksApplication()
            .Name(appName)
            .ProductCode(project.GetAppId())
            .AppVersion(project.GetInformationalVersion());

        CompanyDetails
            .Create(project.GetCompany());

        var fileVersion = new Dictionary<Version, AbsolutePath>();

        foreach (var addinFile in addinFiles)
        {
            var version = NavisworksExtension.GetNavisworksVersion(addinFile);
            fileVersion[version] = addinFile;
        }

        fileVersion = fileVersion
            .OrderBy(e => e.Key)
            .ToDictionary(e => e.Key, e => e.Value);

        for (int i = 0; i < fileVersion.Count; i++)
        {
            var currentVersion = fileVersion.ElementAt(i).Key;
            var currentFile = fileVersion.ElementAt(i).Value;

            AddNavisworksComponentsByFileVersion(project, currentFile, bundleDirectory, currentVersion);
        }
    }

    /// <summary>
    /// Adds Navisworks components to the package for a specific file version.
    /// </summary>
    /// <param name="project">The project containing metadata for the package.</param>
    /// <param name="addinFile">The add-in file to include.</param>
    /// <param name="bundleDirectory">The directory where the bundle is located.</param>
    /// <param name="currentVersion">The current Navisworks version for the component.</param>
    private void AddNavisworksComponentsByFileVersion(Project project, AbsolutePath addinFile, AbsolutePath bundleDirectory,
        Version currentVersion)
    {
        var appName = project.Name;

        var moduleName = ((string)addinFile).Replace(bundleDirectory, ".");

        var version = currentVersion.ToString(2);

        var componentsBuilder = Components
            .CreateEntry($"{AutodeskProducts.Navisworks} {version}")
            .AppType() // Ensure the AppType is set to ManagedPlugin
            .AppName(appName)
            .ModuleName(moduleName)
            .NavisworksPlatform(currentVersion, currentVersion);

        Serilog.Log.Information($"Component Navisworks {version}: {Path.GetFileName(addinFile)}");
    }
}
