using DoenaSoft.MediaInfoHelper.Contracts;
using DoenaSoft.MediaInfoHelper.DefaultProviders;
using DoenaSoft.MediaInfoHelper.Helpers;

namespace MediaInfoHelper.Tests
{
    [TestClass]
    public class SubtitleExtensionProviderTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            // Reset to default after each test
            MediaInfoConfiguration.SubtitleExtensionProvider = DefaultSubtitleExtensionProvider.Instance;
        }

        [TestMethod]
        public void DefaultSubtitleExtensionProvider_IncludesCommonFormats()
        {
            var extensions = DefaultSubtitleExtensionProvider.Instance.GetSubtitleExtensions().ToList();

            Assert.IsTrue(extensions.Contains("srt"));
            Assert.IsTrue(extensions.Contains("sub"));
            Assert.IsTrue(extensions.Contains("sup"));
            Assert.IsTrue(extensions.Contains("idx"));
        }

        [TestMethod]
        public void DefaultSubtitleExtensionProvider_IncludesAdvancedFormats()
        {
            var extensions = DefaultSubtitleExtensionProvider.Instance.GetSubtitleExtensions().ToList();

            Assert.IsTrue(extensions.Contains("ass"));
            Assert.IsTrue(extensions.Contains("ssa"));
            Assert.IsTrue(extensions.Contains("vtt"));
        }

        [TestMethod]
        public void CustomSubtitleExtensionProvider_CanBeInjected()
        {
            // Arrange
            MediaInfoConfiguration.SubtitleExtensionProvider = new CustomTestSubtitleExtensionProvider();

            // Act
            var extensions = MediaInfoConfiguration.SubtitleExtensionProvider.GetSubtitleExtensions().ToList();

            // Assert
            Assert.AreEqual(2, extensions.Count);
            Assert.IsTrue(extensions.Contains("custom1"));
            Assert.IsTrue(extensions.Contains("custom2"));
        }

        [TestMethod]
        public void SettingNullProvider_FallsBackToDefault()
        {
            // Arrange
            MediaInfoConfiguration.SubtitleExtensionProvider = null;

            // Act
            var extensions = MediaInfoConfiguration.SubtitleExtensionProvider.GetSubtitleExtensions().ToList();

            // Assert
            Assert.IsTrue(extensions.Contains("srt"));
            Assert.IsTrue(extensions.Contains("sub"));
        }

        [TestMethod]
        public void DefaultProvider_ReturnsNonNullCollection()
        {
            var extensions = DefaultSubtitleExtensionProvider.Instance.GetSubtitleExtensions();

            Assert.IsNotNull(extensions);
            Assert.IsTrue(extensions.Any());
        }

        private class CustomTestSubtitleExtensionProvider : ISubtitleExtensionProvider
        {
            public IEnumerable<string> GetSubtitleExtensions()
            {
                return new[] { "custom1", "custom2" };
            }
        }
    }
}
