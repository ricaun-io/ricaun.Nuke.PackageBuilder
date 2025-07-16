namespace ricaun.Nuke.Components
{
    /// <summary>
    /// Provides a builder for creating Navisworks plugin packages using Inno Setup scripts.
    /// Inherits from <see cref="IssAppBundleBuilder"/> and enables installation to the user's AppData directory by default.
    /// </summary>
    public class IssNavisworksBuilder : IssAppBundleBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssNavisworksBuilder"/> class.
        /// Sets <see cref="IssAppBundleBuilder.EnableUserAppData"/> to <c>true</c> by default for Navisworks plugins.
        /// </summary>
        public IssNavisworksBuilder()
        {
            EnableUserAppData = true; // Default to true for Navisworks plugins.
        }
    }
}