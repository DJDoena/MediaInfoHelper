using DoenaSoft.MediaInfoHelper.Contracts;

namespace DoenaSoft.MediaInfoHelper.DefaultProviders;

/// <summary>
/// Default implementation of <see cref="ILanguageProvider"/> that standardizes language codes
/// for German, English, Arabic, Spanish, Japanese and Korean, and provides default weighting.
/// </summary>
public sealed class DefaultLanguageProvider : ILanguageProvider
{
    /// <summary>
    /// Gets the singleton instance of the default language provider.
    /// </summary>
    public static DefaultLanguageProvider Instance { get; }

    static DefaultLanguageProvider()
    {
        Instance = new DefaultLanguageProvider();
    }

    private DefaultLanguageProvider()
    {
    }

    /// <summary>
    /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
    /// </summary>
    /// <summary>
    /// Standardizes language codes to ISO 639-1 format (2-letter codes).
    /// Supports ISO 639-1, ISO 639-2/T (terminology), and ISO 639-2/B (bibliographic) codes.
    /// </summary>
    public string StandardizeLanguage(string language)
    {
        switch (language?.ToLowerInvariant())
        {
            // German
            case "de":
            case "deu":
            case "ger":
                {
                    return "de";
                }
            // English
            case "en":
            case "eng":
                {
                    return "en";
                }
            // Spanish
            case "es":
            case "spa":
                {
                    return "es";
                }
            // French
            case "fr":
            case "fra":
            case "fre":
                {
                    return "fr";
                }
            // Italian
            case "it":
            case "ita":
                {
                    return "it";
                }
            // Portuguese
            case "pt":
            case "por":
                {
                    return "pt";
                }
            // Dutch
            case "nl":
            case "nld":
            case "dut":
                {
                    return "nl";
                }
            // Russian
            case "ru":
            case "rus":
                {
                    return "ru";
                }
            // Chinese
            case "zh":
            case "zho":
            case "chi":
                {
                    return "zh";
                }
            // Japanese
            case "ja":
            case "jap":
            case "jpn":
                {
                    return "ja";
                }
            // Korean
            case "ko":
            case "kor":
                {
                    return "ko";
                }
            // Polish
            case "pl":
            case "pol":
                {
                    return "pl";
                }
            // Swedish
            case "sv":
            case "swe":
                {
                    return "sv";
                }
            // Norwegian
            case "no":
            case "nor":
                {
                    return "no";
                }
            // Danish
            case "da":
            case "dan":
                {
                    return "da";
                }
            // Finnish
            case "fi":
            case "fin":
                {
                    return "fi";
                }
            // Greek
            case "el":
            case "ell":
            case "gre":
                {
                    return "el";
                }
            // Turkish
            case "tr":
            case "tur":
                {
                    return "tr";
                }
            // Arabic
            case "ar":
            case "ara":
                {
                    return "ar";
                }
            // Czech
            case "cs":
            case "ces":
            case "cze":
                {
                    return "cs";
                }
            // Hungarian
            case "hu":
            case "hun":
                {
                    return "hu";
                }
            // Romanian
            case "ro":
            case "ron":
            case "rum":
                {
                    return "ro";
                }
            // Ukrainian
            case "uk":
            case "ukr":
                {
                    return "uk";
                }
            // Hindi
            case "hi":
            case "hin":
                {
                    return "hi";
                }
            // Thai
            case "th":
            case "tha":
                {
                    return "th";
                }
            // Vietnamese
            case "vi":
            case "vie":
                {
                    return "vi";
                }
            // Hebrew
            case "he":
            case "heb":
                {
                    return "he";
                }
            default:
                {
                    return language?.ToLower();
                }
        }
    }

    /// <summary>
    /// Gives weight to languages in order: German, English, Spanish, Arabic, Japanese, Korean and then everything else.
    /// </summary>
    public int GetLanguageWeight(string language)
    {
        switch (language?.ToLower())
        {
            case "de":
                {
                    return 1;
                }
            case "en":
                {
                    return 2;
                }
            case "es":
                {
                    return 3;
                }
            case "ar":
                {
                    return 4;
                }
            case "ja":
                {
                    return 5;
                }
            case "ko":
                {
                    return 6;
                }
            default:
                {
                    return 10 + Math.Abs(language?.GetHashCode() ?? 0);
                }
        }
    }
}
