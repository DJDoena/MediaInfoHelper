using DoenaSoft.MediaInfoHelper.Readers;

namespace DoenaSoft.MediaInfoHelper.Tests;

[TestClass]
public sealed class ReadTests
{
    [TestMethod]
    public void AudioBook()
    {
        var videoInfo = MetaReader.ReadVideoMetaDocument("Spaceballs.mkv.xml");

        Assert.IsNotNull(videoInfo);
        Assert.HasCount(1, videoInfo.Video);
        Assert.HasCount(2, videoInfo.Audio);
        Assert.HasCount(5, videoInfo.Subtitle);
    }

    [TestMethod]
    public void Video()
    {
        var audioInfo = MetaReader.ReadAudioBookMetaDocument("The Martian.xml");

        Assert.IsNotNull(audioInfo);

        Assert.AreEqual("The Martian", audioInfo.Title);
    }
}