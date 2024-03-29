﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazInput
    /// </summary>
    public interface IHazInput : IHazPackageBuilderProject, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Input 
        /// </summary>
        [Parameter]
        string Folder => TryGetValue(() => Folder) ?? "Release";

        /// <summary>
        /// InputDirectory
        /// </summary>
        AbsolutePath InputDirectory => GetInputDirectory(GetPackageBuilderProject());

        /// <summary>
        /// GetInputDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetInputDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
