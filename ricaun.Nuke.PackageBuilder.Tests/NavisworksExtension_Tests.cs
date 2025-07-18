using NUnit.Framework;
using ricaun.Nuke.Extensions;
using System.Reflection;

namespace ricaun.Nuke.PackageBuilder.Tests
{
    public class NavisworksExtension_Tests
    {
        [TestCase(2021, "18.0")]
        [TestCase(2022, "19.0")]
        [TestCase(2023, "20.0")]
        [TestCase(2024, "21.0")]
        [TestCase(2025, "22.0")]
        [TestCase(2026, "23.0")]
        public void NavisworksVersion_ShouldBe(int version, string expectedVersion)
        {
            var location = $@"Example\NavisworksAddin.{version}.dll";
            var NavisworksVersion = NavisworksExtension.GetNavisworksVersion(location);
            System.Console.WriteLine($"{NavisworksVersion.ToString(2)}");
            Assert.AreEqual(expectedVersion, NavisworksVersion.ToString(2));
        }

        [Test]
        public void NavisworksVersion_ShouldBeZero()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var NavisworksVersion = NavisworksExtension.GetNavisworksVersion(location);
            Assert.IsNull(NavisworksVersion);
        }

        [Test]
        public void NavisworksVersion_ShouldHasNot()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Assert.IsFalse(NavisworksExtension.HasNavisworksVersion(location));
        }
    }
}