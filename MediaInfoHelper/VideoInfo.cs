namespace DoenaSoft.MediaInfoHelper
{
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// XML structure to represent a media file with a XSLT stylesheet header.
    /// </summary>
    [XmlRoot("doc")]
    public sealed class Doc
    {
        /// <summary />
        [XmlElement]
        public VideoInfo VideoInfo;

        /// <summary />
        [XmlAnyElement]
        public XmlElement[] Any;
    }

    /// <summary>
    /// XML structure to represent a media file.
    /// </summary>
    [XmlRoot]
    public sealed class VideoInfo
    {
        /// <summary />
        [XmlElement]
        public Episode Episode;

        /// <summary />
        [XmlElement]
        public Video[] Video;

        /// <summary />
        [XmlElement]
        public Audio[] Audio;

        /// <summary />
        [XmlElement]
        public Subtitle[] Subtitle;

        /// <summary />
        [XmlAttribute]
        public uint Duration { get; set; }

        /// <summary />
        [XmlIgnore]
        public bool DurationSpecified { get; set; }
    }

    /// <summary>
    /// XML structure to represent a TV show episode.
    /// </summary>
    [DebuggerDisplay("Series: {SeriesName}, Episode: {EpisodeName}")]
    public sealed class Episode
    {
        /// <summary />
        [XmlAttribute]
        public string SeriesName;

        /// <summary />
        [XmlAttribute]
        public string EpisodeNumber;

        /// <summary />
        [XmlAttribute]
        public string EpisodeName;

        /// <summary />
        public override string ToString()
            => ($"{SeriesName} {EpisodeNumber} {EpisodeName}");
    }

    /// <summary>
    /// XML base structure to represent a media stream.
    /// </summary>
    public abstract class StreamBase
    {
        private string m_Language;

        /// <summary />
        [XmlAttribute]
        public string CodecName;

        /// <summary />
        [XmlAttribute]
        public string CodecLongName;

        /// <summary />
        [XmlAttribute]
        public string Language
        {
            get => (m_Language == "und" || m_Language == "null") ? null : m_Language;
            set => m_Language = value;
        }

        /// <summary />
        [XmlAttribute]
        public string Title;
    }

    /// <summary>
    /// XML structure to represent a video stream.
    /// </summary>
    [DebuggerDisplay("Codec: {CodecName}, Language: {Language}")]
    public sealed class Video : StreamBase
    {
        /// <summary />
        [XmlElement]
        public AspectRatio AspectRatio;
    }

    /// <summary>
    /// XML structure to represent the aspect ratio of a video stream.
    /// </summary>
    [DebuggerDisplay("{Width}:{Height}")]
    public sealed class AspectRatio
    {
        /// <summary />
        [XmlAttribute]
        public ushort Width;

        /// <summary />
        [XmlAttribute]
        public ushort Height;

        /// <summary />
        [XmlAttribute]
        public ushort CodedWidth;

        /// <summary />
        [XmlIgnore]
        public bool CodedWidthSpecified;

        /// <summary />
        [XmlAttribute]
        public ushort CodedHeight;

        /// <summary />
        [XmlIgnore]
        public bool CodedHeightSpecified;

        /// <summary />
        [XmlAttribute]
        public decimal Ratio;

        /// <summary />
        [XmlIgnore]
        public bool RatioSpecified;
    }

    /// <summary>
    /// XML structure to represent an audio stream.
    /// </summary>
    [DebuggerDisplay("Channels: {ChannelLayout}, Language: {Language}")]
    public sealed class Audio : StreamBase
    {
        /// <summary />
        [XmlAttribute]
        public ushort SampleRate;

        /// <summary />
        [XmlAttribute]
        public byte Channels;

        /// <summary />
        [XmlAttribute]
        public string ChannelLayout;
    }

    /// <summary>
    /// XML structure to represent a subtitle stream.
    /// </summary>
    [DebuggerDisplay("Language: {Language}")]
    public sealed class Subtitle : StreamBase
    {
        /// <summary />
        [XmlAttribute]
        public string SubtitleFile;
    }
}