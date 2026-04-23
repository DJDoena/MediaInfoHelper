using DoenaSoft.MediaInfoHelper.Helpers;
using DoenaSoft.ToolBox.Generics;
using FfpXml = DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using NR = global::NReco.VideoInfo;

namespace DoenaSoft.MediaInfoHelper.Readers;

/// <summary>
/// Reads FFProbe metadata from video and subtitle files.
/// Uses <see cref="MediaInfoConfiguration.SubtitleExtensionProvider"/> for configurable subtitle format detection.
/// </summary>
public static class FFProbeReader
{
    /// <summary>
    /// Reads out the video meta information of a video or subtitle file.
    /// </summary>
    public static FfpXml.FFProbeMeta TryGetFFProbe(FileInfo file, out List<FfpXml.FFProbeMeta> additionalSubtitleMediaInfos)
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

    private static FfpXml.FFProbeMeta GetFFProbe(FileInfo fileInfo, out List<FfpXml.FFProbeMeta> additionalSubtitleMediaInfos)
    {
        var mediaInfo = (new NR.FFProbe()).GetMediaInfo(fileInfo.FullName);

        var xml = mediaInfo.Result.CreateNavigator().OuterXml;

        var ffprobe = XmlSerializer<FfpXml.FFProbeMeta>.FromString(xml);

        ffprobe.FileName = fileInfo.Name;

        additionalSubtitleMediaInfos = GetSubtitleMediaInfo(fileInfo).ToList();

        return ffprobe;
    }

    private static IEnumerable<FfpXml.FFProbeMeta> GetSubtitleMediaInfo(FileInfo fileInfo)
    {
        var baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);
        var result = new List<FfpXml.FFProbeMeta>();

        var extensions = MediaInfoConfiguration.SubtitleExtensionProvider.GetSubtitleExtensions();

        foreach (var extension in extensions)
        {
            try
            {
                result.AddRange(GetSubtitleMediaInfoByExtension(fileInfo, baseName, extension));
            }
            catch
            { }
        }

        return result;
    }

    private static IEnumerable<FfpXml.FFProbeMeta> GetSubtitleMediaInfoByExtension(FileInfo fileInfo, string baseName, string extension)
    {
        var searchPattern = $"{baseName}*.{extension}";
        var subtitleFiles = fileInfo.Directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

        var result = subtitleFiles
            .Select(sf => TryGetSubtitleMediaInfo(sf, extension))
            .Where(ff => ff?.streams?.Length > 0);

        return result;
    }

    private static FfpXml.FFProbeMeta TryGetSubtitleMediaInfo(FileInfo subtitleFile, string extension)
    {
        // Special handling for idx/sub pairs
        if (extension.Equals("idx", StringComparison.OrdinalIgnoreCase))
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);
            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sub")))
            {
                return null;
            }
        }
        // Special handling for ifo/sup pairs
        else if (extension.Equals("ifo", StringComparison.OrdinalIgnoreCase))
        {
            var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);
            if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sup")))
            {
                return null;
            }
        }
        // Special handling for SRT files (extract language from filename)
        else if (extension.Equals("srt", StringComparison.OrdinalIgnoreCase))
        {
            return TryGetSrtSubtitleMediaInfo(subtitleFile);
        }

        try
        {
            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);
            var xml = mediaInfo.Result.CreateNavigator().OuterXml;
            var ffprobe = XmlSerializer<FfpXml.FFProbeMeta>.FromString(xml);
            ffprobe.FileName = subtitleFile.Name;
            return ffprobe;
        }
        catch
        {
            return null;
        }
    }

    private static FfpXml.FFProbeMeta TryGetSrtSubtitleMediaInfo(FileInfo subtitleFile)
    {
        var fileName = Path.GetFileNameWithoutExtension(subtitleFile.Name);
        var nameParts = fileName.Split(new[] { '.', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var namePart in nameParts)
        {
            var language = TryDetectLanguageFromPart(namePart);
            if (language != null)
            {
                return CreateSrtProbe(subtitleFile, language);
            }
        }

        return null;
    }

    private static string TryDetectLanguageFromPart(string namePart)
    {
        switch (namePart.ToLower())
        {
            case "de":
            case "deu":
            case "ger":
                return "ger";
            case "en":
            case "eng":
                return "eng";
            case "ar":
            case "ara":
                return "ara";
            case "es":
            case "spa":
                return "spa";
            default:
                return null;
        }
    }

    private static FfpXml.FFProbeMeta CreateSrtProbe(FileInfo subtitleFile, string language)
        => new()
        {
            FileName = subtitleFile.Name,
            streams = new FfpXml.Stream[]
            {
                new()
                {
                    codec_type = "subtitle",
                    tag = new FfpXml.Tag[]
                    {
                        new()
                        {
                            key = "language",
                            value = language,
                        },
                    },
                },
            },
        };
}
