using InnoSetup.ScriptBuilder;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IssRevitBuilder
    /// </summary>
    public class IssRevitBuilder : IssBuilder
    {
        private readonly Project project;

        /// <summary>
        /// IssRevitBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="packageBuilderDirectory"></param>
        /// <param name="issConfiguration"></param>
        public IssRevitBuilder(Project project,
            AbsolutePath packageBuilderDirectory,
            IssConfiguration issConfiguration)
        {
            this.project = project;
            var assembly = project.GetAssemblyGreaterVersion();
            string title = assembly.GetTitle();
            string appCopyright = assembly.GetCopyright();
            string appId = project.GetAppId();

            string app = $"{title}";
            string appPath = $"{project.Name}";
            string appVersion = assembly.GetInformationalVersion();

            string bundle = $"{appPath}.bundle";
            string appPublisher = assembly.GetCompany();
            string appComments = assembly.GetDescription();

            string sourceFiles = $@"{bundle}\*";

            var setup =
                Setup.Create(app)
                    .AppVerName(app)
                    .AppId(appId)
                    .AppVersion(appVersion)
                    .AppPublisher(appPublisher)
                    .AppComments(appComments)
                    .AppCopyright(appCopyright)
                    .DefaultDirName($@"{InnoConstants.CommonAppData}\Autodesk\ApplicationPlugins\{bundle}")
                    .OutputBaseFilename($"{title} {appVersion}")
                    .UninstallDisplayIcon($@"{InnoConstants.App}\unins000.exe")
                    .DisableWelcomePage(YesNo.No)
                    .DisableDirPage(YesNo.Yes)
                    .ShowLanguageDialog(YesNo.No);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / issConfiguration.Icon))
                setup.SetupIconFile(issConfiguration.Icon);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / issConfiguration.Image))
                setup.WizardImageFile(issConfiguration.Image);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / issConfiguration.ImageSmall))
                setup.WizardSmallImageFile(issConfiguration.ImageSmall);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / issConfiguration.Licence))
                setup.LicenseFile(issConfiguration.Licence);

            Files.CreateEntry(source: sourceFiles, destDir: InnoConstants.App)
                .Flags(FileFlags.IgnoreVersion | FileFlags.RecurseSubdirs);

            Languages.CreateEntry(name: issConfiguration.Language.Name,
                messagesFile: issConfiguration.Language.MessagesFile);

            if (issConfiguration.IssLanguageLicences != null)
            {
                foreach (var IssLanguageLicence in issConfiguration.IssLanguageLicences)
                {
                    var language = Languages.CreateEntry(name: IssLanguageLicence.Name,
                        messagesFile: IssLanguageLicence.MessagesFile);

                    if (FileSystemTasks.FileExists(packageBuilderDirectory / IssLanguageLicence.Licence))
                        language.LicenseFile(IssLanguageLicence.Licence);
                }
            }
            Sections.CreateParameterSection("UninstallDelete")
                .CreateEntry()
                .Parameter("Type", "filesandordirs")
                .Parameter("Name", $@"{InnoConstants.App}\*");

            Sections.CreateParameterSection("InstallDelete")
                .CreateEntry()
                .Parameter("Type", "files")
                .Parameter("Name", $@"{InnoConstants.App}\Contents\*");

            Sections.CreateParameterSection("Dirs")
                .CreateEntry()
                .Parameter("Name", $"{InnoConstants.App}")
                .Parameter("Permissions", $"users-full");
        }

        /// <summary>
        /// CreateFile
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string CreateFile(string path)
        {
            if (Path.GetExtension(path) != ".iss")
                path = Path.Combine(path, $"{project.Name}.iss");

            File.WriteAllText(path, ToString(), System.Text.Encoding.UTF8);
            return path;
        }
    }
}