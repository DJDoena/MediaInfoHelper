using System;
using System.IO;
using DoenaSoft.MediaInfoHelper.DataObjects;
using DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml;
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.MediaInfoHelper.Forms;
using DoenaSoft.MediaInfoHelper.Helpers;
using NR = global::NReco.VideoInfo;

namespace DoenaSoft.MediaInfoHelper.Readers
{
    /// <summary>
    /// Class to read out the basic information about a media file.
    /// </summary>
    public sealed class VideoReader
    {
        private readonly bool _manualInput;

        private readonly MediaFile _mediaFile;

        /// <summary />
        /// <param name="mediaFile">the media file</param>
        /// <param name="manualInput">whether or not the user shall be shown an input form for the video length if it cannot be determined automatically</param>
        public VideoReader(MediaFile mediaFile, bool manualInput)
        {
            _mediaFile = mediaFile;
            _manualInput = manualInput;
        }

        /// <summary>
        /// Calculates the running time of a media file.
        /// </summary>
        public void DetermineLength()
        {
            if (this.VideoLengthIsValid())
            {
                return;
            }

            uint seconds = 0;

            if (_mediaFile.FileName.EndsWith(Constants.DvdProfilerFileExtension)
                || _mediaFile.FileName.EndsWith(Constants.YoutubeFileExtension)
                || _mediaFile.FileName.EndsWith(Constants.ManualFileExtension))
            {
                seconds = _mediaFile.LengthSpecified
                   ? _mediaFile.Length
                   : this.GetUserInput();
            }
            else if (File.Exists(_mediaFile.FileName))
            {
                seconds = this.GetLengthFromFile();
            }

            if (seconds > 0)
            {
                _mediaFile.Length = seconds;

                return;
            }

            return;
        }

        private bool VideoLengthIsValid()
        {
            var fi = new FileInfo(_mediaFile.FileName);

            DateTime creationTime;
            if (_mediaFile.FileName.EndsWith(Constants.DvdProfilerFileExtension)
                || _mediaFile.FileName.EndsWith(Constants.YoutubeFileExtension)
                || _mediaFile.FileName.EndsWith(Constants.ManualFileExtension))
            {
                creationTime = _mediaFile.CreationTime;
            }
            else
            {
                creationTime = fi.CreationTimeUtc.Conform();
            }

            if (fi.Exists && _mediaFile.CreationTime != creationTime)
            {
                _mediaFile.CreationTime = creationTime;

                return false;
            }

            var isValid = _mediaFile.LengthSpecified;

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
            var videoLength = this.GetLengthFromMeta();

            if (videoLength > 0)
            {
                return videoLength;
            }

            videoLength = this.GetDuration();

            return videoLength;
        }

        private uint GetLengthFromMeta()
        {
            var xmlFile = _mediaFile.FileName + ".xml";

            if (File.Exists(xmlFile))
            {
                try
                {
                    var info = XmlSerializer<Doc>.Deserialize(xmlFile);

                    return info.VideoInfo.Duration;
                }
                catch
                { }
            }

            return 0;
        }

        private uint GetDuration()
        {
            var mediaInfo = this.GetMediaInfo();

            if (mediaInfo != null)
            {
                var xmlInfo = FFProbeMetaConverter.Convert(mediaInfo, null);

                return xmlInfo.DurationSpecified ? xmlInfo.Duration : 0;
            }

            return 0;
        }

        private FFProbeMeta GetMediaInfo()
        {
            try
            {
                var mediaInfo = (new NR.FFProbe()).GetMediaInfo(_mediaFile.FileName);

                var xml = mediaInfo.Result.CreateNavigator().OuterXml;

                var ffprobe = XmlSerializer<FFProbeMeta>.FromString(xml);

                return ffprobe;
            }
            catch
            {
                return null;
            }
        }
    }
}