using DoenaSoft.MediaInfoHelper.DataObjects.AudioBookMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.ToolBox.Generics;

namespace DoenaSoft.MediaInfoHelper.Writers;

/// <summary/>
public static class MetaWriter
{
    /// <summary/>
    public static void WriteAudioBookMeta(AudioBookMeta meta, string fileName)
    {
        XmlSerializer<AudioBookMeta>.Serialize(fileName, meta);
    }

    /// <summary/>
    public static void WriteAudioBookMetaDocument(AudioBookMeta rootItem, string fileName)
    {
        (new XsltSerializer<AudioBookMeta>(new AudioBookRootItemXsltSerializerDataProvider())).Serialize(fileName, rootItem);
    }

    /// <summary/>
    public static void WriteVideoMeta(VideoMeta meta, string fileName)
    {
        XmlSerializer<VideoMeta>.Serialize(fileName, meta);
    }

    /// <summary/>
    public static void WriteVideoMetaDocument(VideoMeta rootItem, string fileName)
    {
        (new XsltSerializer<VideoMeta>(new VideoMetaXsltSerializerDataProvider())).Serialize(fileName, rootItem);
    }

    /// <summary/>
    public static void WriteFFProbeMeta(FFProbeMeta meta, string fileName)
    {
        XmlSerializer<FFProbeMeta>.Serialize(fileName, meta);
    }
}