namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary />
    public static class LanguageExtensions
    {
        /// <summary>
        /// Standardizes audio track names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<Audio> audios) => audios?
            .Select(a => a.Language)
            .StandardizeLanguage();

        /// <summary>
        /// Standardizes subtitle track names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<Subtitle> sutitles) => sutitles?
            .Select(a => a.Language)
            .StandardizeLanguage();

        /// <summary>
        /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<string> languages) => languages?
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(StandardizeLanguage)
            .Distinct();

        /// <summary>
        /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static string StandardizeLanguage(string language)
        {
            switch (language?.ToLowerInvariant())
            {
                case "de":
                case "deu":
                case "ger":
                    {
                        return "de";
                    }
                case "en":
                case "eng":
                    {
                        return "en";
                    }
                case "ar":
                case "ara":
                    {
                        return "ar";
                    }
                case "es":
                case "spa":
                    {
                        return "es";
                    }
                case "ja":
                case "jap":
                case "jpn":
                    {
                        return "ja";
                    }
                case "ko":
                case "kor":
                    {
                        return "ko";
                    }
                default:
                    {
                        return language?.ToLower();
                    }
            }
        }

        /// <summary>
        /// Gives weight to languages in order:  German, English, Spanish, Arabic, Japanese, Korean and then everything else.
        /// </summary>
        public static int GetLanguageWeight(string language)
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
}