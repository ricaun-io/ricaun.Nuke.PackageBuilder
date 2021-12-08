using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazPackageBuilder : IHazSolution, INukeBuild
    {
        /// <summary>
        /// IExternalApplication Class 
        /// </summary>
        [Parameter]
        string Application => ValueInjectionUtility.TryGetValue(() => Application) ?? "Revit.App";

        /// <summary>
        /// Folder PackageBuilder 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "PackageBuilder";
        AbsolutePath PackageBuilderDirectory => Solution.GetMainProject().Directory / "bin" / Folder;
    }
}
