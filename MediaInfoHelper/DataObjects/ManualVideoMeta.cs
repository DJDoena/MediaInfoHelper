using System.Xml.Serialization;

namespace DoenaSoft.MediaInfoHelper.DataObjects;

/// <summary>
/// XML structure to represent a manual video entry.
/// </summary>
[XmlRoot("ManualVideoInfo")]
public sealed class ManualVideo
{
    /// <summary />
    public string Title { get; set; }

    /// <summary />
    public string Note { get; set; }

    /// <summary />
    public uint RunningTime { get; set; }
}