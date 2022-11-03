namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LanguageExtensions
    {
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<Audio> audios) => audios?.Select(a => a.Language?.ToLower());

        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<Subtitle> sutitles) => sutitles?.Select(a => a.Language?.ToLower());

        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<string> languages) => languages?
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(StandardizeLanguage)
            .Distinct();

        public static string StandardizeLanguage(string language)
        {
            switch (language?.ToLower())
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
                        return language.ToLower();
                    }
            }
        }

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
                default:
                    {
                        return 10 + Math.Abs(language?.GetHashCode() ?? 0);
                    }
            }
        }
    }
}