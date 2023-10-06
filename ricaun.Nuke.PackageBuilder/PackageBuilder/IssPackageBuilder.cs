using InnoSetup.ScriptBuilder;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using System.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IssPackageBuilder
    /// </summary>
    public abstract class IssPackageBuilder : IssBuilder
    {
        /// <summary>
        /// Project
        /// </summary>
        protected Project Project;

        /// <summary>
        /// SetProject
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public IssPackageBuilder Initialize(Project project)
        {
            Project = project;
            return this;
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="packageBuilderDirectory"></param>
        /// <param name="issConfiguration"></param>
        /// <returns></returns>
        public abstract IssPackageBuilder CreatePackage(
            AbsolutePath packageBuilderDirectory,
            IssConfiguration issConfiguration);

        /// <summary>
        /// CreateFile
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string CreateFile(string path)
        {
            if (Path.GetExtension(path) != ".iss")
                path = Path.Combine(path, $"{Project.Name}.iss");

            File.WriteAllText(path, ToString(), System.Text.Encoding.UTF8);
            return path;
        }
    }
}