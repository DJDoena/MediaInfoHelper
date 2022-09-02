namespace DoenaSoft.MediaInfoHelper.Youtube
{
    using System;

    public sealed class YoutubeVideoInfo
    {
        public string Id { get; set; }

        public uint RunningTime { get; set; }

        public string Title { get; set; }

        public DateTime Published { get; set; }
    }
}