namespace ricaun.Nuke.Components
{
    /// <summary>
    /// Provides a builder for creating AutoCAD plugin packages using Inno Setup scripts.
    /// Inherits from <see cref="IssAppBundleBuilder"/> and enables installation to the user's AppData directory by default.
    /// </summary>
    public class IssAutoCADBuilder : IssAppBundleBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssAutoCADBuilder"/> class.
        /// Sets <see cref="IssAppBundleBuilder.EnableUserAppData"/> to <c>true</c> by default for AutoCAD plugins.
        /// </summary>
        public IssAutoCADBuilder()
        {
            EnableUserAppData = true; // Default to true for AutoCAD plugins.
        }
    }
}