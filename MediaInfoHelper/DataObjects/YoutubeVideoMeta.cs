using System.Xml.Serialization;

namespace DoenaSoft.MediaInfoHelper.DataObjects;

/// <summary>
/// XML structure to represent a Youtube video.
/// </summary>
[XmlRoot("YoutubeVideoInfo")]
public sealed class YoutubeVideo
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