using NUnit.Framework;
using ricaun.Nuke.Extensions;
using System.Reflection;

namespace ricaun.Nuke.PackageBuilder.Tests
{
    public class RevitExtension_Tests
    {
        [TestCase(2019)]
        [TestCase(2020)]
        [TestCase(2021)]
        [TestCase(2022)]
        [TestCase(2023)]
        [TestCase(2025)]
        public void RevitVersion_ShouldBe(int version)
        {
            var location = $@"Example\{version}\RevitAddin.PackageBuilder.Example.dll";
            var revitVersion = RevitExtension.GetRevitVersion(location);
            System.Console.WriteLine(revitVersion);
            Assert.AreEqual(version, revitVersion);
        }

        [Test]
        public void RevitVersion_ShouldBeZero()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var revitVersion = RevitExtension.GetRevitVersion(location);
            Assert.AreEqual(0, revitVersion);
        }

        [Test]
        public void RevitVersion_ShouldHasNot()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Assert.IsFalse(RevitExtension.HasRevitVersion(location));
        }
    }
}