namespace DoenaSoft.MediaInfoHelper.Youtube
{
    using System;

    /// <summary>
    /// XML structure to represent a Youtube video.
    /// </summary>
    public sealed class YoutubeVideoInfo
    {
        /// <summary />
        public string Id { get; set; }

        /// <summary />
        public uint RunningTime { get; set; }

        /// <summary />
        public string Title { get; set; }

        /// <summary />
        public DateTime Published { get; set; }
    }
}