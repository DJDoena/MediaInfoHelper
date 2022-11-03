namespace DoenaSoft.MediaInfoHelper.Youtube
{
    /// <summary>
    /// XML structure to represent a manual video entry.
    /// </summary>
    public sealed class ManualVideoInfo
    {
        /// <summary />
        public string Title { get; set; }

        /// <summary />
        public string Note { get; set; }

        /// <summary />
        public uint RunningTime { get; set; }
    }
}