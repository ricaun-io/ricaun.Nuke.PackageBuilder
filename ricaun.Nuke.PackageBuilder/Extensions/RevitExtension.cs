using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ricaun.Nuke.Extensions
{
    public static class RevitExtension
    {
        public static int GetRevitVersion(string dll)
        {
            var assemblyTest = Assembly.Load(File.ReadAllBytes(dll));

            var revit = assemblyTest.GetReferencedAssemblies()
                .FirstOrDefault(e => e.Name.StartsWith("RevitAPI"));

            var version = revit.Version.Major;
            if (version < 2000) version += 2000;

            return version;
        }
    }
}
