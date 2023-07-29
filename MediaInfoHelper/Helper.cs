namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using FFProbeResultXml;
    using NR = NReco.VideoInfo;

    /// <summary>
    /// Helper functions to deal with this library.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Formats a time in seconds into a HH:mm:ss format.
        /// </summary>
        public static string FormatTime(uint totalSeconds)
        {
            var (days, hours, minutes) = GetTimeParts(totalSeconds);

            var text = new StringBuilder();

            if (days > 0)
            {
                text.Append(days);
                text.Append("d ");
            }

            if ((days > 0) || (hours > 0))
            {
                text.Append(hours);
                text.Append("h ");
            }

            text.Append(minutes);
            text.Append("m");

            return text.ToString();
        }

        /// <summary>
        /// Adds up a number of <see cref="uint"/> values.
        /// </summary>
        public static uint Sum(this IEnumerable<uint> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            uint result = 0;

            foreach (var item in source)
            {
                result += item;
            }

            return result;
        }

        /// <summary>
        /// Returns days, hours and minutes of a given time in secon
        /// </summary>
        /// <remarks>when the remaining seconds are more or equal to 30, the  part gets rounded to the next whole number</remarks>
        public static (uint days, uint hours, uint minutes) GetTimeParts(uint totalSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            var days = (uint)timeSpan.Days;

            var hours = (uint)timeSpan.Hours;

            var minutes = (uint)timeSpan.Minutes;

            var seonds = (uint)timeSpan.Seconds;

            if (timeSpan.Milliseconds >= 500)
            {
                seonds++;
            }

            if (seonds >= 30)
            {
                minutes++;
            }

            if (minutes == 60)
            {
                minutes = 0;

                hours++;
            }

            if (hours == 24)
            {
                hours = 0;

                days++;
            }

            return (days, hours, minutes);
        }

        /// <summary>
        /// Reads out the video meta information of a video or subtitle file.
        /// </summary>
        public static FFProbeResult TryGetMediaInfo(FileInfo fileInfo, out List<FFProbeResult> additionalSubtitleMediaInfos)
        {
            try
            {
                var result = GetMediaInfo(fileInfo, out additionalSubtitleMediaInfos);

                return result;
            }
            catch
            {
                additionalSubtitleMediaInfos = null;

                return null;
            }
        }

        private static FFProbeResult GetMediaInfo(FileInfo fileInfo, out List<FFProbeResult> additionalSubtitleMediaInfos)
        {
            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(fileInfo.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = Serializer<FFProbeResult>.FromString(xml);

            ffprobe.FileName = fileInfo.Name;

            additionalSubtitleMediaInfos = GetSubtitleMediaInfo(fileInfo).ToList();

            return ffprobe;
        }

        private static IEnumerable<FFProbeResult> GetSubtitleMediaInfo(FileInfo fileInfo)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);

            var result = new List<FFProbeResult>();

            try
            {
                result.AddRange(GetIdxSubSubtitleMediaInfo(fileInfo, baseName));
            }
            catch
            { }

            try
            {
                result.AddRange(GetIfoSupSubtitleMediaInfo(fileInfo, baseName));
            }
            catch
            { }

            try
            {
                result.AddRange(GetSrtSubtitleMediaInfo(fileInfo, baseName));
            }
            catch
            { }

            return result;
        }

        #region GetIdxSubSubtitleMediaInfo

        private static IEnumerable<FFProbeResult> GetIdxSubSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sup", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(TryGetIdxSubSubtitleMediaInfo)
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeResult TryGetIdxSubSubtitleMediaInfo(FileInfo subtitleFile)
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sub")))
            {
                return null;
            }

            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = Serializer<FFProbeResult>.FromString(xml);

            ffprobe.FileName = subtitleFile.Name;

            return ffprobe;
        }

        #endregion

        #region GetIfoSupSubtitleMediaInfo

        private static IEnumerable<FFProbeResult> GetIfoSupSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sup", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(TryGetIfoSupSubtitleMediaInfo)
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeResult TryGetIfoSupSubtitleMediaInfo(FileInfo subtitleFile)
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sup")))
            {
                return null;
            }

            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = Serializer<FFProbeResult>.FromString(xml);

            ffprobe.FileName = subtitleFile.Name;

            return ffprobe;
        }

        #endregion

        #region GetSrtSubtitleMediaInfo

        private static IEnumerable<FFProbeResult> GetSrtSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.srt", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(sf => TryGetSrtSubtitleMediaInfo(sf, baseName))
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeResult TryGetSrtSubtitleMediaInfo(FileInfo subtitleFile, string baseName)
        {
            var nameParts = Path.GetFileNameWithoutExtension(subtitleFile.Name)
                .Replace(baseName, string.Empty)
                .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var namePart in nameParts)
            {
                switch (namePart.ToLower())
                {
                    case "de":
                    case "deu":
                    case "ger":
                        {
                            return CreateSrtProbe(subtitleFile, "ger");
                        }
                    case "en":
                    case "eng":
                        {
                            return CreateSrtProbe(subtitleFile, "eng");
                        }
                    case "ar":
                    case "ara":
                        {
                            return CreateSrtProbe(subtitleFile, "ara");
                        }
                    case "es":
                    case "spa":
                        {
                            return CreateSrtProbe(subtitleFile, "spa");
                        }
                }
            }

            return null;
        }

        private static FFProbeResult CreateSrtProbe(FileInfo subtitleFile, string language) => new FFProbeResult()
        {
            FileName = subtitleFile.Name,
            streams = new FFProbeResultXml.Stream[]
            {
                new FFProbeResultXml.Stream()
                {
                    codec_type = "subtitle",
                    tag = new Tag[]
                    {
                        new Tag()
                        {
                            key = "language",
                            value = language,
                        },
                    },
                },
            },
        };

        #endregion
    }
}