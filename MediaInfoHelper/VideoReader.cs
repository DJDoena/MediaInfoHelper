namespace DoenaSoft.MediaInfoHelper
{
    using System.IO;
    using FF = FFProbe;

    public sealed class VideoReader
    {
        private readonly bool _manualInput;

        private readonly MediaFileData _mediaFile;

        public VideoReader(MediaFileData mediaFile, bool manualInput)
        {
            _mediaFile = mediaFile;
            _manualInput = manualInput;
        }

        public void DetermineLength()
        {
            if (VideoLengthIsValid())
            {
                return;
            }

            uint seconds = 0;

            if (_mediaFile.FileName.EndsWith(Constants.DvdProfilerFileExtension) || _mediaFile.FileName.EndsWith(Constants.YoutubeFileExtension))
            {
                seconds = _mediaFile.VideoLengthSpecified
                   ? _mediaFile.VideoLength
                   : GetUserInput();
            }
            else if (File.Exists(_mediaFile.FileName))
            {
                seconds = GetLengthFromFile();
            }

            if (seconds > 0)
            {
                _mediaFile.VideoLength = seconds;

                return;
            }

            return;
        }

        private bool VideoLengthIsValid()
        {
            var fi = new FileInfo(_mediaFile.FileName);

            var creationTime = fi.CreationTimeUtc.Conform();

            if (fi.Exists && _mediaFile.CreationTime != creationTime)
            {
                _mediaFile.CreationTime = creationTime;

                return false;
            }

            var isValid = _mediaFile.VideoLengthSpecified;

            return isValid;
        }

        private uint GetUserInput()
        {
            if (!_manualInput)
            {
                return 0;
            }

            using (var timeForm = new TimeForm(_mediaFile.FileName))
            {
                timeForm.ShowDialog();

                return timeForm.Time;
            }
        }

        private uint GetLengthFromFile()
        {
            var videoLength = GetLengthFromMeta();

            if (videoLength > 0)
            {
                return videoLength;
            }

            videoLength = GetDuration();

            return videoLength;
        }

        private uint GetLengthFromMeta()
        {
            var xmlFile = _mediaFile.FileName + ".xml";

            if (File.Exists(xmlFile))
            {
                try
                {
                    var info = Serializer<Doc>.Deserialize(xmlFile);

                    return info.VideoInfo.Duration;
                }
                catch
                { }
            }

            return 0;
        }

        private uint GetDuration()
        {
            var mediaInfo = GetMediaInfo();

            if (mediaInfo != null)
            {
                var xmlInfo = MediaInfo2XmlConverter.Convert(mediaInfo, null);

                return xmlInfo.DurationSpecified ? xmlInfo.Duration : 0;
            }

            return 0;
        }

        private FF.FFProbe GetMediaInfo()
        {
            try
            {
                var mediaInfo = (new NReco.VideoInfo.FFProbe()).GetMediaInfo(_mediaFile.FileName);

                var xml = mediaInfo.Result.CreateNavigator().OuterXml;

                var ffprobe = Serializer<FF.FFProbe>.FromString(xml);

                return ffprobe;
            }
            catch
            {
                return null;
            }
        }
    }
}