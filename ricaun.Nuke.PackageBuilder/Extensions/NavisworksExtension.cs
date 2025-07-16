using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke.Extensions;

/// <summary>
/// Provides extension methods for working with Navisworks assemblies.
/// </summary>
public static class NavisworksExtension
{
    private const string AssemblyName = "Autodesk.Navisworks.Api";

    /// <summary>
    /// Determines whether the specified DLL references a Navisworks assembly and has a version.
    /// </summary>
    /// <param name="dll">The path to the DLL file to check.</param>
    /// <returns>
    /// <c>true</c> if the DLL references a Navisworks assembly and has a version; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasNavisworksVersion(string dll)
    {
        return GetNavisworksVersion(dll) is not null;
    }

    /// <summary>
    /// Gets the major and minor version of the referenced Navisworks assembly from the specified DLL.
    /// </summary>
    /// <param name="dll">The path to the DLL file to inspect.</param>
    /// <returns>
    /// A <see cref="Version"/> object containing the major and minor version of the referenced Navisworks assembly,
    /// or <c>null</c> if the reference is not found.
    /// </returns>
    public static Version GetNavisworksVersion(string dll)
    {
        var assemblyTest = Assembly.Load(File.ReadAllBytes(dll));

        var reference = assemblyTest.GetReferencedAssemblies()
            .FirstOrDefault(e => e.Name.StartsWith(AssemblyName, StringComparison.InvariantCultureIgnoreCase));

        if (reference == null) return null;

        return new Version(reference.Version.Major, reference.Version.Minor);
    }
}
