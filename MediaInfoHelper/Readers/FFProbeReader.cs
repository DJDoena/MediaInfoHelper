using DoenaSoft.ToolBox.Generics;
using FfpXml = DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using NR = global::NReco.VideoInfo;

namespace DoenaSoft.MediaInfoHelper.Reader;

/// <summary/>
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

    private static IEnumerable<FfpXml.FFProbeMeta> GetIdxSubSubtitleMediaInfo(FileInfo fileInfo, string baseName)
    {
        var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sub", SearchOption.TopDirectoryOnly);

        var result = subtitleFiles
            .Select(TryGetIdxSubSubtitleMediaInfo)
            .Where(ff => ff?.streams?.Length > 0);

        return result;
    }

    private static FfpXml.FFProbeMeta TryGetIdxSubSubtitleMediaInfo(FileInfo subtitleFile)
    {
        var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

        if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sub")))
        {
            return null;
        }

        var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

        var xml = mediaInfo.Result.CreateNavigator().OuterXml;

        var ffprobe = XmlSerializer<FfpXml.FFProbeMeta>.FromString(xml);

        ffprobe.FileName = subtitleFile.Name;

        return ffprobe;
    }

    #endregion

    #region GetIfoSupSubtitleMediaInfo

    private static IEnumerable<FfpXml.FFProbeMeta> GetIfoSupSubtitleMediaInfo(FileInfo fileInfo, string baseName)
    {
        var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.sup", SearchOption.TopDirectoryOnly);

        var result = subtitleFiles
            .Select(TryGetIfoSupSubtitleMediaInfo)
            .Where(ff => ff?.streams?.Length > 0);

        return result;
    }

    private static FfpXml.FFProbeMeta TryGetIfoSupSubtitleMediaInfo(FileInfo subtitleFile)
    {
        var subtitleBaseName = Path.GetFileNameWithoutExtension(subtitleFile.Name);

        if (!File.Exists(Path.Combine(subtitleFile.DirectoryName, $"{subtitleBaseName}.sup")))
        {
            return null;
        }

        var mediaInfo = (new NR.FFProbe()).GetMediaInfo(subtitleFile.FullName);

        var xml = mediaInfo.Result.CreateNavigator().OuterXml;

        var ffprobe = XmlSerializer<FfpXml.FFProbeMeta>.FromString(xml);

        ffprobe.FileName = subtitleFile.Name;

        return ffprobe;
    }

    #endregion

    #region GetSrtSubtitleMediaInfo

    private static IEnumerable<FfpXml.FFProbeMeta> GetSrtSubtitleMediaInfo(FileInfo fileInfo, string baseName)
    {
        var subtitleFiles = fileInfo.Directory.GetFiles($"{baseName}*.srt", SearchOption.TopDirectoryOnly);

        var result = subtitleFiles
            .Select(sf => TryGetSrtSubtitleMediaInfo(sf, baseName))
            .Where(ff => ff?.streams?.Length > 0);

        return result;
    }

    private static FfpXml.FFProbeMeta TryGetSrtSubtitleMediaInfo(FileInfo subtitleFile, string baseName)
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

    #endregion
}
