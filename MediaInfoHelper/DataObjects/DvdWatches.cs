using System;
using System.Xml.Serialization;

namespace DoenaSoft.MediaInfoHelper.DataObjects
{
    /// <summary>
    /// XML structure to represent a DVD Profiler profile.
    /// </summary>
    [XmlRoot]
    public sealed class DvdWatches
    {
        /// <summary />
        public string Title;

        /// <summary />
        public DateTime PurchaseDate;

        /// <summary />
        [XmlIgnore]
        public bool PurchaseDateSpecified;

        /// <summary />
        public Event[] Watches;
    }

    /// <summary>
    /// XML structure to represent a DVD Profiler profile event.
    /// </summary>
    public sealed class Event
    {
        /// <summary />
        [XmlElement("EventType")]
        public EventType Type;

        /// <summary />
        public DateTime Timestamp;

        /// <summary />
        public string Note;

        /// <summary />
        public User User;
    }

    /// <summary>
    /// DVD Profiler profile event type.
    /// </summary>
    public enum EventType
    {
        /// <summary />
        Watched,

        /// <summary />
        Returned,

        /// <summary />
        Borrowed,
    }

    /// <summary>
    /// XML structure to represent a DVD Profiler profile event user.
    /// </summary>
    public sealed class User
    {
        /// <summary />
        [XmlAttribute]
        public string FirstName;

        /// <summary />
        [XmlAttribute]
        public string LastName;

        /// <summary />
        [XmlAttribute]
        public string EmailAddress;

        /// <summary />
        [XmlAttribute]
        public string PhoneNumber;
    }
}