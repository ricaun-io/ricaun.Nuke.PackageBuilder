using InnoSetup.ScriptBuilder;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;
using System.IO;

namespace ricaun.Nuke.Components
{
    public class IssRevitBuilder : IssBuilder
    {
        Project project;
        public IssRevitBuilder(Project project)
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

            var brLanguage =
                Languages.CreateEntry(name: "br", messagesFile: @"compiler:Languages\BrazilianPortuguese.isl");

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

        public string CreateFile(string path)
        {
            if (Path.GetExtension(path) != ".iss")
                path = Path.Combine(path, $"{project.Name}.iss");

            File.WriteAllText(path, ToString(), System.Text.Encoding.UTF8);
            return path;
        }
    }
}