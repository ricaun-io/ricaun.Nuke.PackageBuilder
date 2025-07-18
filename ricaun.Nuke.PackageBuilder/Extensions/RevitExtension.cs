using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// Provides extension methods for working with RevitAPI referenced assemblies.
    /// </summary>
    public static class RevitExtension
    {
        private const string AssemblyName = "RevitAPI";

        /// <summary>
        /// Determines whether the specified DLL references a RevitAPI assembly and has a valid Revit version.
        /// </summary>
        /// <param name="dll">The path to the DLL file to inspect.</param>
        /// <returns>
        /// <c>true</c> if the DLL references a RevitAPI assembly and the version is greater than 0; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasRevitVersion(string dll)
        {
            return GetRevitVersion(dll) > 0;
        }

        /// <summary>
        /// Gets the major version of the RevitAPI assembly referenced by the specified DLL.
        /// </summary>
        /// <param name="dll">The path to the DLL file to inspect.</param>
        /// <returns>
        /// The major version of the referenced RevitAPI assembly, or 0 if not found.
        /// If the version is less than 2000, 2000 is added to the version.
        /// </returns>
        public static int GetRevitVersion(string dll)
        {
            var assemblyTest = Assembly.Load(File.ReadAllBytes(dll));

            var revit = assemblyTest.GetReferencedAssemblies()
                .FirstOrDefault(e => e.Name.StartsWith(AssemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (revit == null) return 0;

            var version = revit.Version.Major;
            if (version < 2000) version += 2000;

            return version;
        }
    }
}
