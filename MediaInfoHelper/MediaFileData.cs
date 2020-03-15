using System;

namespace DoenaSoft.MediaInfoHelper
{
    public sealed class MediaFileData
    {
        private DateTime _creationTime;

        private uint _videoLength;

        public string FileName { get; }

        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                if (_creationTime != value)
                {
                    _creationTime = value;
                    HasChanged = true;
                }
            }
        }

        public uint VideoLength
        {
            get => _videoLength;
            set
            {
                if (_videoLength != value)
                {
                    _videoLength = value;
                    HasChanged = true;
                }
            }
        }

        public bool VideoLengthSpecified => VideoLength > 0;

        public bool HasChanged { get; private set; }

        public MediaFileData(string fileName, DateTime creationTime, uint videoLength)
        {
            FileName = fileName;
            CreationTime = creationTime;
            VideoLength = videoLength;
            HasChanged = false;
        }
    }
}