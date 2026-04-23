using DoenaSoft.MediaInfoHelper.Contracts;
using DoenaSoft.MediaInfoHelper.DefaultProviders;
using DoenaSoft.MediaInfoHelper.Helpers;

namespace MediaInfoHelper.Tests
{
    [TestClass]
    public class LanguageProviderTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            // Reset to default after each test
            MediaInfoConfiguration.LanguageProvider = DefaultLanguageProvider.Instance;
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesGerman()
        {
            var result = LanguageExtensions.StandardizeLanguage("deu");
            Assert.AreEqual("de", result);

            result = LanguageExtensions.StandardizeLanguage("ger");
            Assert.AreEqual("de", result);
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesEnglish()
        {
            var result = LanguageExtensions.StandardizeLanguage("eng");
            Assert.AreEqual("en", result);
        }

        [TestMethod]
        public void DefaultLanguageProvider_ReturnsCorrectWeights()
        {
            Assert.AreEqual(1, LanguageExtensions.GetLanguageWeight("de"));
            Assert.AreEqual(2, LanguageExtensions.GetLanguageWeight("en"));
            Assert.AreEqual(3, LanguageExtensions.GetLanguageWeight("es"));
        }

        [TestMethod]
        public void CustomLanguageProvider_CanBeInjected()
        {
            // Arrange
            MediaInfoConfiguration.LanguageProvider = new CustomTestLanguageProvider();

            // Act
            var result = LanguageExtensions.StandardizeLanguage("french");
            var weight = LanguageExtensions.GetLanguageWeight("fr");

            // Assert
            Assert.AreEqual("fr", result);
            Assert.AreEqual(100, weight);
        }

        [TestMethod]
        public void SettingNullProvider_FallsBackToDefault()
        {
            // Arrange
            MediaInfoConfiguration.LanguageProvider = null;

            // Act
            var result = LanguageExtensions.StandardizeLanguage("deu");

            // Assert
            Assert.AreEqual("de", result);
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesFrench()
        {
            Assert.AreEqual("fr", LanguageExtensions.StandardizeLanguage("fr"));
            Assert.AreEqual("fr", LanguageExtensions.StandardizeLanguage("fra"));
            Assert.AreEqual("fr", LanguageExtensions.StandardizeLanguage("fre"));
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesItalian()
        {
            Assert.AreEqual("it", LanguageExtensions.StandardizeLanguage("it"));
            Assert.AreEqual("it", LanguageExtensions.StandardizeLanguage("ita"));
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesRussian()
        {
            Assert.AreEqual("ru", LanguageExtensions.StandardizeLanguage("ru"));
            Assert.AreEqual("ru", LanguageExtensions.StandardizeLanguage("rus"));
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesChinese()
        {
            Assert.AreEqual("zh", LanguageExtensions.StandardizeLanguage("zh"));
            Assert.AreEqual("zh", LanguageExtensions.StandardizeLanguage("zho"));
            Assert.AreEqual("zh", LanguageExtensions.StandardizeLanguage("chi"));
        }

        [TestMethod]
        public void DefaultLanguageProvider_StandardizesDutch()
        {
            Assert.AreEqual("nl", LanguageExtensions.StandardizeLanguage("nl"));
            Assert.AreEqual("nl", LanguageExtensions.StandardizeLanguage("nld"));
            Assert.AreEqual("nl", LanguageExtensions.StandardizeLanguage("dut"));
        }

        private class CustomTestLanguageProvider : ILanguageProvider
        {
            public string StandardizeLanguage(string language)
            {
                if (language?.ToLower() == "french")
                    return "fr";
                return language?.ToLower();
            }

            public int GetLanguageWeight(string language)
            {
                if (language?.ToLower() == "fr")
                    return 100;
                return 999;
            }
        }
    }
}
