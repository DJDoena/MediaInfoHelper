using System;

namespace DoenaSoft.MediaInfoHelper
{
    public sealed class MediaFileData
    {
        public string FileName { get; }

        public DateTime CreationTime { get; set; }

        public uint VideoLength { get; set; }

        public bool VideoLengthSpecified => VideoLength > 0;

        public MediaFileData(string fileName, DateTime creationTime, uint videoLength)
        {
            FileName = fileName;
            CreationTime = creationTime;
            VideoLength = videoLength;
        }
    }
}