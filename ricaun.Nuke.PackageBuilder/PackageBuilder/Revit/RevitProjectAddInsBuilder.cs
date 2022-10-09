using Autodesk.PackageBuilder;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// RevitProjectAddInsBuilder
    /// </summary>
    public class RevitProjectAddInsBuilder : RevitAddInsBuilder
    {
        /// <summary>
        /// RevitProjectAddInsBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="assemblyFile"></param>
        /// <param name="application"></param>
        /// <param name="applicationType"></param>
        /// <param name="vendorId"></param>
        /// <param name="vendorDescription"></param>
        public RevitProjectAddInsBuilder(Project project, string assemblyFile, string application,
            string applicationType, string vendorId, string vendorDescription)
        {
            var addInId = project.GetAppId();
            var name = project.Name;
            var assemblyName = Path.GetFileName(assemblyFile);

            if (vendorId == null) vendorId = name;
            if (vendorDescription == null) vendorDescription = name;

            AddIn.CreateEntry(applicationType)
                .Name(name)
                .AddInId(addInId)
                .Assembly(assemblyName)
                .FullClassName($"{name}.{application}")
                .VendorId(vendorId)
                .VendorDescription(vendorDescription);

            Serilog.Log.Information($"Create AddIns Application: {assemblyName} {application} {applicationType}");
        }
    }
}
