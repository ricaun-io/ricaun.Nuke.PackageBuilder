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
        private const string IMAGE = "image.bmp";
        private const string IMAGESMALL = "imageSmall.bmp";
        private const string ICON = "icon.ico";
        private const string LICENSE = "License.rtf";
        private const string LICENSE_BR = "License-br.rtf";

        Project project;

        /// <summary>
        /// IssRevitBuilder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="packageBuilderDirectory"></param>
        public IssRevitBuilder(Project project, AbsolutePath packageBuilderDirectory)
        {
            this.project = project;
            var assembly = project.GetAssemblyGreaterVersion();
            string product = assembly.GetProduct();
            string appCopyright = assembly.GetCopyright();
            string appId = project.GetAppId();

            string app = $"{product}";
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
                    .OutputBaseFilename($"{product} {appVersion}")
                    .UninstallDisplayIcon($@"{InnoConstants.App}\unins000.exe")
                    .DisableWelcomePage(YesNo.No)
                    .DisableDirPage(YesNo.Yes)
                    .ShowLanguageDialog(YesNo.No);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / ICON))
                setup.SetupIconFile(ICON);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / IMAGE))
                setup.WizardImageFile(IMAGE);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / IMAGESMALL))
                setup.WizardSmallImageFile(IMAGESMALL);

            if (FileSystemTasks.FileExists(packageBuilderDirectory / LICENSE))
                setup.LicenseFile(LICENSE);

            /*
            setup
                .SetupIconFile("icon.ico")
                .WizardImageFile("image.bmp")
                .WizardSmallImageFile("imageSmall.bmp")
                .LicenseFile("EULA.rtf");
            */

            Files.CreateEntry(source: sourceFiles, destDir: InnoConstants.App)
                .Flags(FileFlags.IgnoreVersion | FileFlags.RecurseSubdirs);

            Languages.CreateEntry(name: "en", messagesFile: @"compiler:Default.isl");

            var brLanguage = Languages.CreateEntry(name: "br", messagesFile: @"compiler:Languages\BrazilianPortuguese.isl");

            if (FileSystemTasks.FileExists(packageBuilderDirectory / LICENSE_BR))
                brLanguage.LicenseFile(LICENSE_BR);

            //brLanguage.LicenseFile("CLUF.rtf");

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