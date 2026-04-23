using DoenaSoft.MediaInfoHelper.Contracts;
using DoenaSoft.MediaInfoHelper.DefaultProviders;
using DoenaSoft.MediaInfoHelper.Helpers;

namespace MediaInfoHelper.Examples;

/// <summary>
/// Example implementation of a custom language provider that prioritizes French and Italian.
/// </summary>
public class EuropeanLanguageProvider : ILanguageProvider
{
    /// <summary>
    /// Standardizes common European language codes to ISO 639-1 format.
    /// </summary>
    public string StandardizeLanguage(string language)
    {
        switch (language?.ToLowerInvariant())
        {
            // French
            case "fr":
            case "fra":
            case "fre":
            case "french":
                return "fr";

            // Italian
            case "it":
            case "ita":
            case "italian":
                return "it";

            // German
            case "de":
            case "deu":
            case "ger":
            case "german":
                return "de";

            // English
            case "en":
            case "eng":
            case "english":
                return "en";

            // Spanish
            case "es":
            case "spa":
            case "spanish":
                return "es";

            // Portuguese
            case "pt":
            case "por":
            case "portuguese":
                return "pt";

            // Dutch
            case "nl":
            case "nld":
            case "dut":
            case "dutch":
                return "nl";

            default:
                return language?.ToLower();
        }
    }

    /// <summary>
    /// Prioritizes European languages with French and Italian at the top.
    /// </summary>
    public int GetLanguageWeight(string language)
    {
        switch (language?.ToLower())
        {
            case "fr":
                return 1;  // French - highest priority
            case "it":
                return 2;  // Italian
            case "en":
                return 3;  // English
            case "de":
                return 4;  // German
            case "es":
                return 5;  // Spanish
            case "pt":
                return 6;  // Portuguese
            case "nl":
                return 7;  // Dutch
            default:
                return 100 + Math.Abs(language?.GetHashCode() ?? 0);
        }
    }
}

/// <summary>
/// Example showing how to configure and use a custom language provider.
/// </summary>
public static class LanguageProviderExample
{
    /// <summary>
    /// Configures the library to use the European language provider.
    /// </summary>
    public static void ConfigureForEuropeanContent()
    {
        // Set the custom provider at application startup
        MediaInfoConfiguration.LanguageProvider = new EuropeanLanguageProvider();
    }

    /// <summary>
    /// Resets the language provider to the default implementation.
    /// </summary>
    public static void ResetToDefault()
    {
        // Reset to default behavior
        MediaInfoConfiguration.LanguageProvider = DefaultLanguageProvider.Instance;
    }

    /// <summary>
    /// Demonstrates how to configure and use a custom language provider.
    /// </summary>
    public static void DemoUsage()
    {
        // Configure for European content
        ConfigureForEuropeanContent();

        // Now all language operations use the European provider
        var french = LanguageExtensions.StandardizeLanguage("french");  // Returns "fr"
        var weight = LanguageExtensions.GetLanguageWeight("fr");  // Returns 1

        Console.WriteLine($"Standardized: {french}, Weight: {weight}");

        // Reset when done (if needed)
        ResetToDefault();
    }
}
