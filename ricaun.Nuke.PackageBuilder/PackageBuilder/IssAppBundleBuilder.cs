using InnoSetup.ScriptBuilder;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IssAutodeskBuilder
    /// </summary>
    public class IssAppBundleBuilder : IssPackageBuilder
    {
        /// <summary>
        /// Gets or sets a value indicating whether to enable installation to the user's AppData directory.
        /// </summary>
        public bool EnableUserAppData { get; set; }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="packageBuilderDirectory"></param>
        /// <param name="issConfiguration"></param>
        /// <returns></returns>
        public override IssPackageBuilder CreatePackage(AbsolutePath packageBuilderDirectory, IssConfiguration issConfiguration)
        {
            string title = Project.GetTitle();

            if (string.IsNullOrWhiteSpace(issConfiguration.Title) == false)
                title = issConfiguration.Title;

            string appCopyright = Project.GetCopyright();
            string appId = Project.GetAppId();

            string app = $"{title}";
            string appPath = $"{Project.Name}";
            string appVersion = Project.GetInformationalVersion();

            string bundle = $"{appPath}.bundle";
            string appPublisher = Project.GetCompany();
            string appComments = Project.GetDescription();

            string sourceFiles = $@"{bundle}\*";

            string appDataFolder = InnoConstants.Shell.CommonAppData;
            if (EnableUserAppData)
                appDataFolder = InnoConstants.Shell.UserAppData;

            var setup =
                Setup.Create(app)
                    .AppVerName(app)
                    .AppId(appId)
                    .AppVersion(appVersion)
                    .AppPublisher(appPublisher)
                    .AppComments(appComments)
                    .AppCopyright(appCopyright)
                    .UsePreviousAppDir(YesNo.No) // Force to install in the new folder.
                    .DefaultDirName($@"{appDataFolder}\Autodesk\ApplicationPlugins\{bundle}")
                    .OutputBaseFilename($"{title}.{appVersion}")
                    .UninstallDisplayIcon($@"{InnoConstants.Directories.App}\unins000.exe")
                    .DisableWelcomePage(YesNo.No)
                    .DisableDirPage(YesNo.Yes)
                    .ShowLanguageDialog(YesNo.No);

            if (EnableUserAppData)
                setup.PrivilegesRequired(PrivilegesRequired.Lowest); // Use lowest privileges for UserAppData installation.

            if ((packageBuilderDirectory / issConfiguration.Icon).FileExists())
                setup.SetupIconFile(issConfiguration.Icon);

            if ((packageBuilderDirectory / issConfiguration.Image).FileExists())
                setup.WizardImageFile(issConfiguration.Image);

            if ((packageBuilderDirectory / issConfiguration.ImageSmall).FileExists())
                setup.WizardSmallImageFile(issConfiguration.ImageSmall);

            if ((packageBuilderDirectory / issConfiguration.Licence).FileExists())
            {
                setup.LicenseFile(issConfiguration.Licence);
                (packageBuilderDirectory / issConfiguration.Licence)
                    .UpdateKeyAppName(app)
                    .UpdateKeyAppVersion(appVersion)
                    .UpdateKeyAppPublisher(appPublisher)
                    .UpdateKeyCompany(appPublisher)
                    .UpdateKeyYear();
            }

            Files.CreateEntry(source: sourceFiles, destDir: $@"\\?\{InnoConstants.Directories.App}")
                .Flags(FileFlags.IgnoreVersion | FileFlags.RecurseSubdirs);

            Languages.CreateEntry(name: issConfiguration.Language.Name,
                messagesFile: issConfiguration.Language.MessagesFile);

            if (issConfiguration.IssLanguageLicences != null)
            {
                foreach (var IssLanguageLicence in issConfiguration.IssLanguageLicences)
                {
                    var language = Languages.CreateEntry(name: IssLanguageLicence.Name,
                        messagesFile: IssLanguageLicence.MessagesFile);

                    if ((packageBuilderDirectory / IssLanguageLicence.Licence).FileExists())
                    {
                        language.LicenseFile(IssLanguageLicence.Licence);
                        (packageBuilderDirectory / IssLanguageLicence.Licence)
                            .UpdateKeyAppName(app)
                            .UpdateKeyAppVersion(appVersion)
                            .UpdateKeyAppPublisher(appPublisher)
                            .UpdateKeyCompany(appPublisher)
                            .UpdateKeyYear();
                    }
                }
            }
            Sections.CreateParameterSection("UninstallDelete")
                .CreateEntry()
                .Parameter("Type", "filesandordirs")
                .Parameter("Name", $@"{InnoConstants.Directories.App}\*");

            // this is used to delete the files inside the Contents folder, to remove old Loader files.
            var InstallDelete = Sections.CreateParameterSection("InstallDelete")
                .CreateEntry()
                .Parameter("Type", "files")
                .Parameter("Name", $@"{InnoConstants.Directories.App}\Contents\*");

            if (EnableUserAppData)
            {
                // Delete PackageContents.xml after install in the `CommonAppData`
                InstallDelete
                    .CreateEntry()
                    .Parameter("Type", "files")
                    .Parameter("Name", $@"{InnoConstants.Shell.CommonAppData}\Autodesk\ApplicationPlugins\{bundle}\PackageContents.xml");
            }

            if (!EnableUserAppData)
            {
                Sections.CreateParameterSection("Dirs")
                    .CreateEntry()
                    .Parameter("Name", $"{InnoConstants.Directories.App}")
                    .Parameter("Permissions", $"users-full");
            }

            return this;
        }
    }
}