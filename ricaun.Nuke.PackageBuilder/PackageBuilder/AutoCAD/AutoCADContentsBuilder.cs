using Autodesk.PackageBuilder;
using Autodesk.PackageBuilder.Extensible.AutoCAD;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Components;

/// <summary>
/// Provides functionality to build AutoCAD package contents for deployment.
/// </summary>
public class AutoCADContentsBuilder : PackageContentsBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoCADContentsBuilder"/> class.
    /// Builds the AutoCAD package contents based on the provided project, bundle directory, and add-in files.
    /// </summary>
    /// <param name="project">The project containing metadata for the package.</param>
    /// <param name="bundleDirectory">The directory where the bundle is located.</param>
    /// <param name="addinFiles">A collection of add-in files to include in the package.</param>
    /// <param name="middleVersion">Indicates whether to include middle versions in the package.</param>
    /// <param name="lastVersions">Indicates whether to include the last version in the package.</param>
    public AutoCADContentsBuilder(Project project, AbsolutePath bundleDirectory,
        IEnumerable<AbsolutePath> addinFiles,
        bool middleVersion, bool lastVersions)
    {
        var appName = project.Name;

        ApplicationPackage
            .Create()
            .AutoCADApplication()
            .Name(appName)
            .ProductCode(project.GetAppId())
            .AppVersion(project.GetInformationalVersion());

        CompanyDetails
            .Create(project.GetCompany());

        var fileVersion = new Dictionary<Version, AbsolutePath>();

        foreach (var addinFile in addinFiles)
        {
            var version = AutoCADExtension.GetAutoCADVersion(addinFile);
            fileVersion[version] = addinFile;
        }

        fileVersion = fileVersion
            .OrderBy(e => e.Key)
            .ToDictionary(e => e.Key, e => e.Value);

        for (int i = 0; i < fileVersion.Count; i++)
        {
            var currentVersion = fileVersion.ElementAt(i).Key;
            var currentFile = fileVersion.ElementAt(i).Value;

            var nextVersion = i + 1 < fileVersion.Count ? fileVersion.ElementAt(i + 1).Key : null;

            if (nextVersion is null && !lastVersions)
            {
                nextVersion = currentVersion;
            }

            AddAutoCADComponentsByFileVersion(project, currentFile, bundleDirectory, currentVersion, nextVersion, middleVersion);
        }
    }

    /// <summary>
    /// Adds AutoCAD components to the package for a specific file version.
    /// </summary>
    /// <param name="project">The project containing metadata for the package.</param>
    /// <param name="addinFile">The add-in file to include.</param>
    /// <param name="bundleDirectory">The directory where the bundle is located.</param>
    /// <param name="currentVersion">The current AutoCAD version for the component.</param>
    /// <param name="nextVersion">The next AutoCAD version for the component, or <c>null</c> if not applicable.</param>
    /// <param name="includeMiddleVersion">Indicates whether to include middle versions.</param>
    /// <param name="globalLocalCommand">Optional global/local command string for the component.</param>
    private void AddAutoCADComponentsByFileVersion(Project project, AbsolutePath addinFile, AbsolutePath bundleDirectory,
        Version currentVersion, Version nextVersion = null, bool includeMiddleVersion = false, string globalLocalCommand = null)
    {
        var appName = project.Name;

        var moduleName = ((string)addinFile).Replace(bundleDirectory, ".");

        var version = currentVersion.ToString(2);

        var componentsBuilder = Components
            .CreateEntry($"{AutodeskProducts.AutoCAD} {version}")
            .AppName(appName)
            .ModuleName(moduleName)
            .AutoCADPlatform(currentVersion, nextVersion, includeMiddleVersion);

        componentsBuilder.LoadOnAppearance(true);

        if (!string.IsNullOrWhiteSpace(globalLocalCommand))
        {
            componentsBuilder.Commands(globalLocalCommand);
        }

        Serilog.Log.Information($"Component AutoCAD {version}: {Path.GetFileName(addinFile)}");
    }
}