namespace DoenaSoft.MediaInfoHelper
{
    using System;

    /// <summary>
    /// Structure to represent the base data of a media file.
    /// </summary>
    public sealed class MediaFileData
    {
        private DateTime _creationTime;

        private uint _videoLength;

        /// <summary />
        public string FileName { get; }

        /// <summary />
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                if (_creationTime != value)
                {
                    _creationTime = value;

                    this.HasChanged = true;
                }
            }
        }

        /// <summary />
        public uint VideoLength
        {
            get => _videoLength;
            set
            {
                if (_videoLength != value)
                {
                    _videoLength = value;

                    this.HasChanged = true;
                }
            }
        }

        /// <summary />
        public bool VideoLengthSpecified => this.VideoLength > 0;

        /// <summary />
        public bool HasChanged { get; private set; }

        /// <summary />
        public MediaFileData(string fileName, DateTime creationTime, uint videoLength)
        {
            this.FileName = fileName;
            this.CreationTime = creationTime;
            this.VideoLength = videoLength;
            this.HasChanged = false;
        }
    }
}