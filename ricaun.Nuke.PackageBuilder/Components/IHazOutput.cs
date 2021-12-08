using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazOutput : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Release 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Output";

        AbsolutePath OutputDirectory => Solution.GetMainProject().Directory / "bin" / Folder;
    }
}
