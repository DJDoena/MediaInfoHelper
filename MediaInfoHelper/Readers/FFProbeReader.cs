using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using DoenaSoft.MediaInfoHelper.Helpers;
using NR = global::NReco.VideoInfo;

namespace DoenaSoft.MediaInfoHelper.Reader
{
    /// <summary/>
    public static class FFProbeReader
    {
        /// <summary>
        /// Reads out the video meta information of a video or subtitle file.
        /// </summary>
        public static FFProbeMeta TryGetFFProbe(FileInfo file, out List<FFProbeMeta> additionalSubtitleMediaInfos)
        {
            try
            {
                var result = GetFFProbe(file, out additionalSubtitleMediaInfos);

                return result;
            }
            catch
            {
                additionalSubtitleMediaInfos = null;

                return null;
            }
        }

        private static FFProbeMeta GetFFProbe(FileInfo fileInfo, out List<FFProbeMeta> additionalSubtitleMediaInfos)
        {
            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(fileInfo.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = XmlSerializer<FFProbeMeta>.FromString(xml);

            ffprobe.FileName = fileInfo.Name;

            additionalSubtitleMediaInfos = GetSubtitleMediaInfo(fileInfo).ToList();

            return ffprobe;
        }

        private static IEnumerable<FFProbeMeta> GetSubtitleMediaInfo(FileInfo fileInfo)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);

            var result = new List<FFProbeMeta>();

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

        private static IEnumerable<FFProbeMeta> GetIdxSubSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sup", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(TryGetIdxSubSubtitleMediaInfo)
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeMeta TryGetIdxSubSubtitleMediaInfo(FileInfo subtitleFile)
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sub")))
            {
                return null;
            }

            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = XmlSerializer<FFProbeMeta>.FromString(xml);

            ffprobe.FileName = subtitleFile.Name;

            return ffprobe;
        }

        #endregion

        #region GetIfoSupSubtitleMediaInfo

        private static IEnumerable<FFProbeMeta> GetIfoSupSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sup", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(TryGetIfoSupSubtitleMediaInfo)
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeMeta TryGetIfoSupSubtitleMediaInfo(FileInfo subtitleFile)
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sup")))
            {
                return null;
            }

            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = XmlSerializer<FFProbeMeta>.FromString(xml);

            ffprobe.FileName = subtitleFile.Name;

            return ffprobe;
        }

        #endregion

        #region GetSrtSubtitleMediaInfo

        private static IEnumerable<FFProbeMeta> GetSrtSubtitleMediaInfo(FileInfo fileInfo, string baseName)
        {
            var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.srt", SearchOption.TopDirectoryOnly);

            var result = subtitleFiles
                .Select(sf => TryGetSrtSubtitleMediaInfo(sf, baseName))
                .Where(ff => ff?.streams?.Length > 0);

            return result;
        }

        private static FFProbeMeta TryGetSrtSubtitleMediaInfo(FileInfo subtitleFile, string baseName)
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

        private static FFProbeMeta CreateSrtProbe(FileInfo subtitleFile, string language) => new FFProbeMeta()
        {
            FileName = subtitleFile.Name,
            streams = new DataObjects.FFProbeMetaXml.Stream[]
            {
                new DataObjects.FFProbeMetaXml.Stream()
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
