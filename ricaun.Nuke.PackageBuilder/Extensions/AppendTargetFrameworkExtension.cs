using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// AppendTargetFrameworkExtension
    /// </summary>
    public static class AppendTargetFrameworkExtension
    {
        /// <summary>
        /// RemoveAppendTargetFrameworkDirectory
        /// </summary>
        /// <param name="contentsDirectory"></param>
        public static void RemoveAppendTargetFrameworkDirectory(AbsolutePath contentsDirectory)
        {
            Globbing.GlobDirectories(contentsDirectory, "**/net*")
                .ForEach(targetFrameworkDirectory =>
                {
                    var directoryName = targetFrameworkDirectory.Name;
                    Serilog.Log.Information($"RemoveAppendTargetFrameworkDirectory: {directoryName} - {targetFrameworkDirectory}");
                    if (targetFrameworkDirectory.Exists())
                    {
                        if (targetFrameworkDirectory.Parent.ContainsFile("*") == false)
                        {
                            Serilog.Log.Information($"CopyDirectoryRecursively: {directoryName} to {targetFrameworkDirectory.Parent.Name}");
                            Serilog.Log.Information($"RemoveTargetFrameworkDirectory: {directoryName} move to {targetFrameworkDirectory.Parent.Name}");
                            FileSystemTasks.CopyDirectoryRecursively(targetFrameworkDirectory, targetFrameworkDirectory.Parent, DirectoryExistsPolicy.Merge);
                            targetFrameworkDirectory.DeleteDirectory();
                        }
                    }
                });
        }
    }
}
