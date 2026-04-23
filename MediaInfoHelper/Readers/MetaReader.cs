using DoenaSoft.MediaInfoHelper.DataObjects.AudioBookMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.ToolBox.Generics;

namespace DoenaSoft.MediaInfoHelper.Readers;

/// <summary/>
public static class MetaReader
{
    /// <summary/>
    public static VideoMeta ReadVideoMetaDocument(string fileName)
    {
        var doc = XsltSerializer<VideoInfoDocument>.Deserialize(fileName);

        return doc.VideoInfo;
    }

    /// <summary/>
    public static VideoMeta ReadVideoMeta(string fileName)
    {
        var videoMeta = XmlSerializer<VideoMeta>.Deserialize(fileName);

        return videoMeta;
    }

    /// <summary/>
    public static AudioBookMeta ReadAudioBookMetaDocument(string fileName)
    {
        var doc = XsltSerializer<AudioBookDocument>.Deserialize(fileName);

        return doc.Mp3Meta;
    }

    /// <summary/>
    public static AudioBookMeta ReadAudioBookMeta(string fileName)
    {
        var audioMeta = XmlSerializer<AudioBookMeta>.Deserialize(fileName);

        return audioMeta;
    }

    /// <summary/>
    public static FFProbeMeta ReadFFProbeMeta(string fileName)
    {
        var ffProbeMeta = XmlSerializer<FFProbeMeta>.Deserialize(fileName);

        return ffProbeMeta;
    }
}