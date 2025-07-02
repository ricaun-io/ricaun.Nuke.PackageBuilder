using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke.Extensions;

/// <summary>
/// Provides extension methods for working with AutoCAD assemblies.
/// </summary>
public static class AutoCADExtension
{
    private const string AssemblyName = "AcDbMgd";

    /// <summary>
    /// Determines whether the specified DLL references an AutoCAD assembly and has a version.
    /// </summary>
    /// <param name="dll">The path to the DLL file to check.</param>
    /// <returns>
    /// <c>true</c> if the DLL references an AutoCAD assembly and has a version; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasAutoCADVersion(string dll)
    {
        return GetAutoCADVersion(dll) is not null;
    }

    /// <summary>
    /// Gets the major and minor version of the referenced AutoCAD assembly from the specified DLL.
    /// </summary>
    /// <param name="dll">The path to the DLL file to inspect.</param>
    /// <returns>
    /// A <see cref="Version"/> object containing the major and minor version of the referenced AutoCAD assembly,
    /// or <c>null</c> if the reference is not found.
    /// </returns>
    public static Version GetAutoCADVersion(string dll)
    {
        var assemblyTest = Assembly.Load(File.ReadAllBytes(dll));

        var reference = assemblyTest.GetReferencedAssemblies()
            .FirstOrDefault(e => e.Name.StartsWith(AssemblyName, StringComparison.InvariantCultureIgnoreCase));

        if (reference == null) return null;

        return new Version(reference.Version.Major, reference.Version.Minor);
    }
}
