using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml
{
    public partial class VideoInfoDocument
    {
        /// <summary />
        [XmlAnyElement]
        public XmlElement[] AnyElements;

        /// <summary />
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttributes;
    }

    public partial class VideoMeta
    {
        /// <summary />
        [XmlAnyElement]
        public XmlElement[] AnyElements;

        /// <summary />
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttributes;
    }

    [DebuggerDisplay("Series: {SeriesName}, Episode: {EpisodeName}")]
    public partial class Episode
    {
        /// <summary />
        public override string ToString()
            => ($"{this.SeriesName} {this.EpisodeNumber} {this.EpisodeName}");
    }

    public partial class StreamBase
    {
        /// <summary/>
        public string GetLanguage()
        {
            var language = this.Language?.ToLowerInvariant()?.Trim();

            switch (language)
            {
                case "":
                case "und":
                case "null":
                    {
                        return null;
                    }
                default:
                    {
                        return this.Language;
                    }
            }
        }
    }

    [DebuggerDisplay("Codec: {CodecName}, Language: {Language}")]
    public partial class Video
    {
    }

    [DebuggerDisplay("{Width}:{Height}")]
    public partial class AspectRatio
    {
    }

    [DebuggerDisplay("Channels: {ChannelLayout}, Language: {Language}")]
    public partial class Audio
    {
    }

    [DebuggerDisplay("Language: {Language}")]
    public partial class Subtitle
    {
    }
}