using Nuke.Common;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// Represents a Navisworks package builder component for Nuke build automation.
    /// Inherits from <see cref="IHazPackageBuilderProject"/> and <see cref="INukeBuild"/>.
    /// </summary>
    public interface IHazNavisworksPackageBuilder : IHazPackageBuilderProject, INukeBuild
    {

    }
}
