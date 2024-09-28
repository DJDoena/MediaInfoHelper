namespace DoenaSoft.MediaInfoHelper.DataObjects;

/// <summary>
/// Structure to represent the base data of a media file.
/// </summary>
public sealed class MediaFile
{
    private DateTime _creationTime;

    private uint _length;

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
    public uint Length
    {
        get => _length;
        set
        {
            if (_length != value)
            {
                _length = value;

                this.HasChanged = true;
            }
        }
    }

    /// <summary />
    public bool LengthSpecified => this.Length > 0;

    /// <summary />
    public bool HasChanged { get; private set; }

    /// <summary />
    public MediaFile(string fileName, DateTime creationTime, uint videoLength)
    {
        this.FileName = fileName;
        this.CreationTime = creationTime;
        this.Length = videoLength;
        this.HasChanged = false;
    }
}