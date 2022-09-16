namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using DS = FFProbe;
    using NR = NReco.VideoInfo;

    public static class Helper
    {
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

        public static DS.FFProbe TryGetMediaInfo(FileInfo fileInfo, out List<DS.FFProbe> additionalSubtitleMediaInfos)
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

        private static DS.FFProbe GetMediaInfo(FileInfo fileInfo, out List<DS.FFProbe> additionalSubtitleMediaInfos)
        {
            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(fileInfo.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = Serializer<DS.FFProbe>.FromString(xml);

            ffprobe.FileName = fileInfo.Name;

            additionalSubtitleMediaInfos = GetSubtitleMediaInfo(fileInfo).ToList();

            return ffprobe;
        }

        private static IEnumerable<DS.FFProbe> GetSubtitleMediaInfo(FileInfo fileInfo)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);

            var result = new List<DS.FFProbe>();

            try
            {
                result.AddRange(GetIdxSubSubtitleMediaInfo(fileInfo, baseName));
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

        private static IEnumerable<DS.FFProbe> GetIdxSubSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.idx", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(TryGetIdxSubSubtitleMediaInfo)
                .Where(ff => ff != null);

            return result;
        }

        private static DS.FFProbe TryGetIdxSubSubtitleMediaInfo(FileInfo subtitleFile)
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sub")))
            {
                return null;
            }

            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = Serializer<DS.FFProbe>.FromString(xml);

            ffprobe.FileName = subtitleFile.Name;

            return ffprobe;
        }

        private static IEnumerable<DS.FFProbe> GetSrtSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.srt", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(sf => TryGetSrtSubtitleMediaInfo(sf, baseName))
                .Where(ff => ff != null);

            return result;
        }

        private static DS.FFProbe TryGetSrtSubtitleMediaInfo(FileInfo subtitleFile, string baseName)
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

        private static DS.FFProbe CreateSrtProbe(FileInfo subtitleFile, string language) => new DS.FFProbe()
        {
            FileName = subtitleFile.Name,
            streams = new DS.Stream[]
            {
                new DS.Stream()
                {
                    codec_type = "subtitle",
                    tag = new DS.Tag[]
                    {
                        new DS.Tag()
                        {
                            key = "language",
                            value = language,
                        },
                    },
                },
            },
        };

        //private static DS.FFProbe GetMediaInfo(FileInfo fileInfo)
        //{
        //    try
        //    {
        //        var mediaInfo = (new NR.FFProbe()).GetMediaInfo(fileInfo.FullName);

        //        var xml = mediaInfo.Result.CreateNavigator().OuterXml;

        //        var ffprobe = Serializer<DS.FFProbe>.FromString(xml);

        //        return ffprobe;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}