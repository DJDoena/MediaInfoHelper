using DoenaSoft.MediaInfoHelper.Contracts;
using VMXml = DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;

namespace DoenaSoft.MediaInfoHelper.Helpers
{
    /// <summary>
    /// Extension methods for language standardization and manipulation.
    /// Uses <see cref="MediaInfoConfiguration.LanguageProvider"/> for customizable language handling.
    /// </summary>
    public static class LanguageExtensions
    {
        /// <summary>
        /// Standardizes audio track languages using the configured <see cref="ILanguageProvider"/> and returns distinct values.
        /// </summary>
        public static IEnumerable<string> GetDistinctLanguages(this IEnumerable<VMXml.Audio> audios)
            => audios?
                .Select(a => a.GetLanguage())
                .StandardizeLanguage()
                .Distinct();

        /// <summary>
        /// Standardizes subtitle track languages using the configured <see cref="ILanguageProvider"/> and returns distinct values.
        /// </summary>
        public static IEnumerable<string> GetDistinctLanguages(this IEnumerable<VMXml.Subtitle> sutitles)
            => sutitles?
                .Select(a => a.GetLanguage())
                .StandardizeLanguage()
                .Distinct();

        /// <summary>
        /// Standardizes languages for audio tracks using the configured <see cref="ILanguageProvider"/>.
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
        /// Standardizes languages for subtitle tracks using the configured <see cref="ILanguageProvider"/>.
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
        /// Standardizes language codes using the configured <see cref="ILanguageProvider"/> and returns distinct values.
        /// </summary>
        public static IEnumerable<string> StandardizeLanguage(this IEnumerable<string> languages)
            => languages?
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(StandardizeLanguage)
                .Distinct();

        /// <summary>
        /// Standardizes language names using the configured <see cref="ILanguageProvider"/>.
        /// </summary>
        public static string StandardizeLanguage(string language)
            => MediaInfoConfiguration.LanguageProvider.StandardizeLanguage(language);

        /// <summary>
        /// Gives weight to languages for sorting using the configured <see cref="ILanguageProvider"/>.
        /// Lower values indicate higher priority.
        /// </summary>
        public static int GetLanguageWeight(string language)
            => MediaInfoConfiguration.LanguageProvider.GetLanguageWeight(language);
    }
}