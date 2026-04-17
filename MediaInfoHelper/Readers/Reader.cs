using System.Xml;
using DoenaSoft.MediaInfoHelper.DataObjects.AudioBookMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.ToolBox.Generics;

namespace DoenaSoft.MediaInfoHelper.Readers;

public static class MetaReader
{
    public static VideoMeta ReadVideoMetaDocument(string fileName)
    {
        using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        using var xmlReader = new XmlTextReader(stream);

        var settings = new XmlReaderSettings()
        {
            DtdProcessing = DtdProcessing.Parse,
        };

        var doc = XmlSerializer<VideoInfoDocument>.Deserialize(xmlReader);

        return doc.VideoInfo;
    }

    public static VideoMeta ReadVideoMeta(string fileName)
    {
        var videoMeta = XmlSerializer<VideoMeta>.Deserialize(fileName);

        return videoMeta;
    }

    public static AudioBookMeta ReadAudioBookMetaDocument(string fileName)
    {
        using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        using var xmlReader = new XmlTextReader(stream);

        var settings = new XmlReaderSettings()
        {
            DtdProcessing = DtdProcessing.Parse,
        };

        var doc = XmlSerializer<AudioBookDocument>.Deserialize(xmlReader);

        return doc.Mp3Meta;
    }

    public static AudioBookMeta ReadAudioBookMeta(string fileName)
    {
        var audioMeta = XmlSerializer<AudioBookMeta>.Deserialize(fileName);

        return audioMeta;
    }
}