using DoenaSoft.MediaInfoHelper.Readers;

namespace DoenaSoft.MediaInfoHelper.Tests;

[TestClass]
public sealed class ReaderTests
{
    [TestMethod]
    public void ReadSpaceballs()
    {
        var videoInfo = MetaReader.ReadVideoMetaDocument("Spaceballs.mkv.xml");

        Assert.IsNotNull(videoInfo);
        Assert.HasCount(1, videoInfo.Video);
    }

    [TestMethod]
    public void ReadTheMartian()
    {
        var audioInfo = MetaReader.ReadAudioBookMetaDocument("The Martian.xml");

        Assert.IsNotNull(audioInfo);

        Assert.AreEqual("The Martian", audioInfo.Title);
    }
}