namespace ricaun.Nuke.Components
{
    /// <summary>
    /// Provides a builder for creating Revit-specific application bundles using Inno Setup scripts.
    /// Inherits from <see cref="IssAppBundleBuilder"/> to leverage common app bundle packaging functionality.
    /// </summary>
    public class IssRevitBuilder : IssAppBundleBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssRevitBuilder"/> class.
        /// Sets <see cref="IssAppBundleBuilder.EnableUserAppData"/> to <c>true</c> by default for Revit plugins,
        /// ensuring installation to the user's AppData directory for compatibility with Revit's plugin loading mechanism.
        /// </summary>
        public IssRevitBuilder()
        {
            // Default to true for Revit plugins, allowing installation to the user's AppData directory.
            EnableUserAppData = true;
            Serilog.Log.Warning("IssRevitBuilder set 'EnableUserAppData' to true for Revit plugins by default. This force the AppBundle to be installed in the current user AppData folder.");
        }
    }
}