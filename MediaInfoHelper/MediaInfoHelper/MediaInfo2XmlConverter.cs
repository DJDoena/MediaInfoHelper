﻿namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FF = FFProbe;

    public static class MediaInfo2XmlConverter
    {
        private static readonly CultureInfo _cultureInfo;

        private static string _originalLanguage;

        static MediaInfo2XmlConverter()
        {
            _cultureInfo = CultureInfo.GetCultureInfo("en-US");
        }

        public static string GetLanguage(this IEnumerable<FF.Tag> tags)
            => tags?.FirstOrDefault(tag => tag.key == "language")?.value ?? _originalLanguage;

        public static VideoInfo Convert(FF.FFProbe ffprobe, string originalLanguage)
        {
            _originalLanguage = originalLanguage;

            try
            {
                var info = TryConvert(ffprobe);

                return info;
            }
            finally
            {
                _originalLanguage = null;
            }
        }

        private static VideoInfo TryConvert(FF.FFProbe ffprobe)
        {
            var info = new VideoInfo()
            {
                Duration = ConvertDuration(ffprobe.format?.duration),
                Video = ffprobe.streams?.Where(IsVideo).Select(GetXmlVideo).ToArray(),
                Audio = ffprobe.streams?.Where(IsAudio).Select(GetXmlAudio).ToArray(),
                Subtitle = ffprobe.streams?.Where(IsSubtile).Select(GetXmlSubtitle).ToArray(),
            };

            info.DurationSpecified = info.Duration > 0;

            if (info.Video?.Length == 0)
            {
                info.Video = null;
            }

            if (info.Audio?.Length == 0)
            {
                info.Audio = null;
            }

            if (info.Subtitle?.Length == 0)
            {
                info.Subtitle = null;
            }

            return info;
        }

        private static bool IsSubtile(FF.Stream stream)
            => stream?.codec_type == "subtitle";

        private static Subtitle GetXmlSubtitle(FF.Stream stream)
            => new Subtitle()
            {
                CodecName = stream.codec_name,
                CodecLongName = stream.codec_long_name,
                Language = stream.tag.GetLanguage(),
                Title = stream.tag.GetTitle()
            };

        private static bool IsAudio(FF.Stream stream)
            => stream?.codec_type == "audio";

        private static Audio GetXmlAudio(FF.Stream stream)
            => new Audio()
            {
                CodecName = stream.codec_name,
                CodecLongName = stream.codec_long_name,
                SampleRate = stream.sample_rate,
                Channels = stream.channels,
                ChannelLayout = stream.channel_layout,
                Language = stream.tag.GetLanguage(),
                Title = stream.tag.GetTitle(),
            };

        private static bool IsVideo(FF.Stream stream)
            => stream?.codec_type == "video";

        private static Video GetXmlVideo(FF.Stream stream)
            => new Video()
            {
                CodecName = stream.codec_name,
                CodecLongName = stream.codec_long_name,
                AspectRatio = GetAspectRatio(stream),
                Language = stream.tag.GetLanguage(),
                Title = stream.tag.GetTitle(),
            };

        private static AspectRatio GetAspectRatio(FF.Stream stream)
        {
            var ratio = new AspectRatio()
            {
                Width = stream.width,
                Height = stream.height,
                CodedWidth = stream.coded_width,
                CodedHeight = stream.coded_height,
                Ratio = GetAspectRatio(stream.display_aspect_ratio),
            };

            ratio.CodedWidthSpecified = ratio.CodedWidth != 0 && ratio.Width != ratio.CodedWidth;
            ratio.CodedHeightSpecified = ratio.CodedHeight != 0 && ratio.Height != ratio.CodedHeight;

            if ((ratio.Ratio == 0) && (ratio.Height > 0))
            {
                ratio.Ratio = CalulateRatio(ratio.Width, ratio.Height);
            }

            ratio.RatioSpecified = ratio.Ratio > 0;

            return ratio;
        }

        private static uint ConvertDuration(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
            {
                return 0;
            }
            else if (TryParseDouble(duration, out var seconds))
            {
                return (uint)seconds;
            }
            else if (TryParseTimeSpan(duration, out var timeSpan))
            {
                return (uint)timeSpan.TotalSeconds;
            }

            return 0;
        }

        private static decimal GetAspectRatio(string aspectRatio)
        {
            if (string.IsNullOrWhiteSpace(aspectRatio))
            {
                return 0;
            }

            var split = aspectRatio.Split(':');

            if (split.Length != 2)
            {
                return 0;
            }
            else if ((uint.TryParse(split[0], out var width)) && (uint.TryParse(split[1], out var height)) && (height > 0))
            {
                var ratio = CalulateRatio(width, height);

                return ratio;
            }

            return 0;
        }

        private static decimal CalulateRatio(decimal width, decimal height)
            => Math.Round(width / height, 2, MidpointRounding.AwayFromZero);

        private static bool TryParseTimeSpan(string duration, out TimeSpan timeSpan)
        {
            var indexOf = duration.IndexOf('.');

            var reduced = indexOf > 0 ? duration.Substring(0, indexOf) : duration;

            var success = TimeSpan.TryParse(reduced, _cultureInfo, out timeSpan);

            return success;
        }

        private static bool TryParseDouble(string duration, out double seconds)
            => double.TryParse(duration, NumberStyles.AllowDecimalPoint, _cultureInfo, out seconds);

        private static string GetTitle(this IEnumerable<FF.Tag> tags)
            => tags?.FirstOrDefault(tag => tag.key == "title")?.value;
    }
}