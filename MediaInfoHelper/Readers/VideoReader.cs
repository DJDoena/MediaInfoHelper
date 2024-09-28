using DoenaSoft.MediaInfoHelper.DataObjects;
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.MediaInfoHelper.Helpers;
using DoenaSoft.ToolBox.Generics;
using FfpXml = DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using NR = global::NReco.VideoInfo;

namespace DoenaSoft.MediaInfoHelper.Readers;

/// <summary>
/// Class to read out the basic information about a media file.
/// </summary>
public sealed class VideoReader
{
    /// <summary />
    public delegate uint GetRunningTime(string fileName);

    private readonly GetRunningTime _getRunningTime;

    private readonly MediaFile _mediaFile;

    /// <summary />
    public VideoReader(MediaFile mediaFile
        , GetRunningTime getRunningTime = null)
    {
        _mediaFile = mediaFile;
        _getRunningTime = getRunningTime;
    }

    /// <summary>
    /// Calculates the running time of a media file.
    /// </summary>
    public void DetermineLength()
    {
        if (this.VideoLengthIsValid())
        {
            return;
        }

        uint seconds = 0;

        if (_mediaFile.FileName.EndsWith(Constants.DvdProfilerFileExtension)
            || _mediaFile.FileName.EndsWith(Constants.YoutubeFileExtension)
            || _mediaFile.FileName.EndsWith(Constants.ManualFileExtension))
        {
            if (_mediaFile.LengthSpecified)
            {
                seconds = _mediaFile.Length;
            }
            else if (_getRunningTime != null)
            {
                seconds = _getRunningTime(_mediaFile.FileName);
            }
        }
        else if (File.Exists(_mediaFile.FileName))
        {
            seconds = this.GetLengthFromFile();
        }

        if (seconds > 0)
        {
            _mediaFile.Length = seconds;

            return;
        }

        return;
    }

    private bool VideoLengthIsValid()
    {
        var fi = new FileInfo(_mediaFile.FileName);

        DateTime creationTime;
        if (_mediaFile.FileName.EndsWith(Constants.DvdProfilerFileExtension)
            || _mediaFile.FileName.EndsWith(Constants.YoutubeFileExtension)
            || _mediaFile.FileName.EndsWith(Constants.ManualFileExtension))
        {
            creationTime = _mediaFile.CreationTime;
        }
        else
        {
            creationTime = fi.CreationTimeUtc.Conform();
        }

        if (fi.Exists && _mediaFile.CreationTime != creationTime)
        {
            _mediaFile.CreationTime = creationTime;

            return false;
        }

        var isValid = _mediaFile.LengthSpecified;

        return isValid;
    }

    private uint GetLengthFromFile()
    {
        var videoLength = this.GetLengthFromMeta();

        if (videoLength > 0)
        {
            return videoLength;
        }

        videoLength = this.GetDuration();

        return videoLength;
    }

    private uint GetLengthFromMeta()
    {
        var xmlFile = _mediaFile.FileName + ".xml";

        if (File.Exists(xmlFile))
        {
            try
            {
                var info = XmlSerializer<VideoInfoDocument>.Deserialize(xmlFile);

                return info.VideoInfo.Duration;
            }
            catch
            { }
        }

        return 0;
    }

    private uint GetDuration()
    {
        var mediaInfo = this.GetMediaInfo();

        if (mediaInfo != null)
        {
            var xmlInfo = FFProbeMetaConverter.Convert(mediaInfo, null);

            return xmlInfo.DurationSpecified ? xmlInfo.Duration : 0;
        }

        return 0;
    }

    private FfpXml.FFProbeMeta GetMediaInfo()
    {
        try
        {
            var mediaInfo = (new NR.FFProbe()).GetMediaInfo(_mediaFile.FileName);

            var xml = mediaInfo.Result.CreateNavigator().OuterXml;

            var ffprobe = XmlSerializer<FfpXml.FFProbeMeta>.FromString(xml);

            return ffprobe;
        }
        catch
        {
            return null;
        }
    }
}