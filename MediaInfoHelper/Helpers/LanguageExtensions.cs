using VMXml = DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;

namespace DoenaSoft.MediaInfoHelper.Helpers
{
    /// <summary />
    public static class LanguageExtensions
    {
        /// <summary>
        /// Standardizes audio track names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> GetDistinctLanguages(this IEnumerable<VMXml.Audio> audios)
            => audios?
                .Select(a => a.GetLanguage())
                .StandardizeLanguage()
                .Distinct();

        /// <summary>
        /// Standardizes subtitle track names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> GetDistinctLanguages(this IEnumerable<VMXml.Subtitle> sutitles)
            => sutitles?
                .Select(a => a.GetLanguage())
                .StandardizeLanguage()
                .Distinct();

        /// <summary>
        /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<VMXml.Audio> StandardizeLanguage(this IEnumerable<VMXml.Audio> audios)
            => audios?
                .Where(a => a != null)
                .Select(a => new VMXml.Audio()
                {
                    ChannelLayout = a.ChannelLayout,
                    Channels = a.Channels,
                    CodecLongName = a.CodecLongName,
                    CodecName = a.CodecName,
                    Language = StandardizeLanguage(a.GetLanguage()),
                    SampleRate = a.SampleRate,
                    Title = a.Title,
                });

        /// <summary>
        /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<VMXml.Subtitle> StandardizeLanguage(this IEnumerable<VMXml.Subtitle> subtitles)
            => subtitles?
                .Where(s => s != null)
                .Select(s => new VMXml.Subtitle()
                {
                    CodecLongName = s.CodecLongName,
                    CodecName = s.CodecName,
                    Language = StandardizeLanguage(s.GetLanguage()),
                    SubtitleFile = s.SubtitleFile,
                    Title = s.Title,
                });

        /// <summary>
        /// Standardizes language names for German, English, Arabic, Spanish, Japanese and Korean.
        /// </summary>
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<string> languages)
            => languages?
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