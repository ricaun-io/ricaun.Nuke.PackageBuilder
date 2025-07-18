using NUnit.Framework;
using ricaun.Nuke.Extensions;
using System.Reflection;

namespace ricaun.Nuke.PackageBuilder.Tests
{
    public class AutoCADExtension_Tests
    {
        [TestCase(2019, "23.0")] // net47
        [TestCase(2020, "23.1")]
        [TestCase(2021, "24.0")] // net48
        [TestCase(2022, "24.1")]
        [TestCase(2023, "24.2")]
        [TestCase(2024, "24.3")]
        [TestCase(2025, "25.0")] // net8.0
        [TestCase(2026, "25.1")]
        public void AutoCADVersion_ShouldBe(int version, string expectedVersion)
        {
            var location = $@"Example\AutoCADAddin.{version}.dll";
            var AutoCADVersion = AutoCADExtension.GetAutoCADVersion(location);
            System.Console.WriteLine($"{AutoCADVersion.ToString(2)}");
            Assert.AreEqual(expectedVersion, AutoCADVersion.ToString(2));
        }

        [Test]
        public void AutoCADVersion_ShouldBeZero()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var AutoCADVersion = AutoCADExtension.GetAutoCADVersion(location);
            Assert.IsNull(AutoCADVersion);
        }

        [Test]
        public void AutoCADVersion_ShouldHasNot()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Assert.IsFalse(AutoCADExtension.HasAutoCADVersion(location));
        }
    }
}