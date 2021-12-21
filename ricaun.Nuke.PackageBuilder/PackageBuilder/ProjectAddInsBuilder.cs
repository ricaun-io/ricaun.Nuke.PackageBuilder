using Autodesk.PackageBuilder;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;
using System.Reflection;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ProjectAddInsBuilder
    /// </summary>
    public class ProjectAddInsBuilder : RevitAddInsBuilder
    {
        /// <summary>
        /// ProjectAddInsBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="assemblyFile"></param>
        /// <param name="application"></param>
        public ProjectAddInsBuilder(Project project, string assemblyFile, string application)
        {
            var addInId = project.GetAppId();
            var name = project.Name;
            var assemblyName = Path.GetFileName(assemblyFile);

            AddIn.CreateEntry()
                .Name(name)
                .AddInId(addInId)
                .Assembly(assemblyName)
                .FullClassName($"{name}.{application}")
                .VendorId(name)
                .VendorDescription(name);
        }
    }
}
