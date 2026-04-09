using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components;

/// <summary>
/// Provides extension methods for <see cref="RevitProjectAddInsBuilder"/> to configure Revit add-ins.
/// </summary>
public static class RevitProjectAddInsBuilderExtension
{
    /// <summary>
    /// For more information, see:
    /// https://help.autodesk.com/view/RVT/2026/ENU/?guid=Revit_API_Revit_API_Developers_Guide_Introduction_Add_In_Integration_Add_in_Dependency_Isolation_html
    /// </summary>
    private const int ManifestSettingsSupportVersion = 2026;

    /// <summary>
    /// Configures the Revit add-in context isolation settings for the specified builder.
    /// </summary>
    /// <param name="builder">The <see cref="RevitProjectAddInsBuilder"/> instance to configure.</param>
    /// <param name="file">The file path of the Revit add-in.</param>
    /// <param name="revitContextIsolation">Indicates whether context isolation is enabled.</param>
    /// <param name="revitContextName">The name of the Revit context.</param>
    /// <param name="revitContextVersion">The minimum Revit version required for context isolation.</param>
    /// <returns>The configured <see cref="RevitProjectAddInsBuilder"/> instance.</returns>
    public static RevitProjectAddInsBuilder AddInContextIsolation(this RevitProjectAddInsBuilder builder,
        string file,
        bool revitContextIsolation,
        string revitContextName,
        int revitContextVersion)
    {
        if (revitContextIsolation || revitContextName != null)
        {
            var fileRevitVersion = RevitExtension.GetRevitVersion(file);
            if (fileRevitVersion >= revitContextVersion && fileRevitVersion >= ManifestSettingsSupportVersion)
            {
                var manifestSettings = builder.CreateManifestSettings();
                manifestSettings.UseRevitContext = false;
                manifestSettings.ContextName = revitContextName;
                Serilog.Log.Information($"Create AddIns ManifestSettings.UseRevitContext in Revit {fileRevitVersion} with ContextName '{revitContextName}'");
            }
        }

        return builder;
    }
}
