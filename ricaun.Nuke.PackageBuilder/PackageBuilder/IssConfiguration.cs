namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IssConfiguration
    /// </summary>
    public class IssConfiguration
    {
        /// <summary>
        /// IMAGE
        /// </summary>
        public const string IMAGE = "image.bmp";
        /// <summary>
        /// IMAGESMALL
        /// </summary>
        public const string IMAGESMALL = "imageSmall.bmp";
        /// <summary>
        /// ICON
        /// </summary>
        public const string ICON = "icon.ico";
        /// <summary>
        /// LICENSE
        /// </summary>
        public const string LICENSE = "License.txt";

        /// <summary>
        /// Title (default null)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Image (default <see cref="IMAGE"/>)
        /// </summary>
        public string Image { get; set; } = IMAGE;

        /// <summary>
        /// Small Image (default <see cref="IMAGESMALL"/>)
        /// </summary>
        public string ImageSmall { get; set; } = IMAGESMALL;

        /// <summary>
        /// Icon (default <see cref="ICON"/>)
        /// </summary>
        public string Icon { get; set; } = ICON;

        /// <summary>
        /// Licence (default <see cref="LICENSE"/>)
        /// </summary>
        public string Licence { get; set; } = LICENSE;

        /// <summary>
        /// Language (default <see cref="IssLanguage"/>)
        /// </summary>
        public IssLanguage Language { get; set; } = new IssLanguage();

        /// <summary>
        /// IssLanguages
        /// </summary>
        public IssLanguageLicence[] IssLanguageLicences { get; set; }
    }

    /// <summary>
    /// IssLanguage
    /// </summary>
    public class IssLanguage
    {
        /// <summary>
        /// Name (default "en")
        /// </summary>
        public string Name { get; set; } = "en";

        /// <summary>
        /// MessagesFile (default "compiler:Default.isl")
        /// </summary>
        public string MessagesFile { get; set; } = "compiler:Default.isl";

    }

    /// <summary>
    /// IssLanguageLicence
    /// </summary>
    public class IssLanguageLicence : IssLanguage
    {
        /// <summary>
        /// Licence (default <see cref="IssConfiguration.LICENSE"/>)
        /// </summary>
        public string Licence { get; set; } = IssConfiguration.LICENSE;
    }
}