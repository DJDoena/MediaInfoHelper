using DoenaSoft.MediaInfoHelper.Readers;
using DoenaSoft.MediaInfoHelper.Writers;

namespace DoenaSoft.MediaInfoHelper.Tests;

[TestClass]
public sealed class WriteReadTests
{
    [TestMethod]
    public void VideoDocument()
    {
        var source = MetaReader.ReadVideoMetaDocument("Spaceballs.mkv.xml");

        MetaWriter.WriteVideoMetaDocument(source, "videoDoc.xml");

        var videoInfo = MetaReader.ReadVideoMetaDocument("videoDoc.xml");

        Assert.IsNotNull(videoInfo);
        Assert.HasCount(1, videoInfo.Video);
        Assert.HasCount(2, videoInfo.Audio);
        Assert.HasCount(5, videoInfo.Subtitle);
    }

    [TestMethod]
    public void AudioBookDocument()
    {
        var source = MetaReader.ReadAudioBookMetaDocument("The Martian.xml");

        MetaWriter.WriteAudioBookMetaDocument(source, "audioDoc.xml");

        var audioInfo = MetaReader.ReadAudioBookMetaDocument("audioDoc.xml");

        Assert.IsNotNull(audioInfo);

        Assert.AreEqual("The Martian", audioInfo.Title);
    }

    [TestMethod]
    public void Video()
    {
        var source = MetaReader.ReadVideoMetaDocument("Spaceballs.mkv.xml");

        MetaWriter.WriteVideoMeta(source, "video.xml");

        var videoInfo = MetaReader.ReadVideoMeta("video.xml");

        Assert.IsNotNull(videoInfo);
        Assert.HasCount(1, videoInfo.Video);
        Assert.HasCount(2, videoInfo.Audio);
        Assert.HasCount(5, videoInfo.Subtitle);
    }

    [TestMethod]
    public void AudioBook()
    {
        var source = MetaReader.ReadAudioBookMetaDocument("The Martian.xml");

        MetaWriter.WriteAudioBookMeta(source, "audio.xml");

        var audioInfo = MetaReader.ReadAudioBookMeta("audio.xml");

        Assert.IsNotNull(audioInfo);

        Assert.AreEqual("The Martian", audioInfo.Title);
    }
}
