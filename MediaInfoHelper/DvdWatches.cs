namespace DoenaSoft.MediaInfoHelper.DVDProfiler
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot]
    public sealed class DvdWatches
    {
        public string Title;

        public DateTime PurchaseDate;

        [XmlIgnore]
        public bool PurchaseDateSpecified;

        public Event[] Watches;
    }

    public sealed class Event
    {
        [XmlElement("EventType")]
        public EventType Type;

        public DateTime Timestamp;

        public string Note;

        public User User;
    }

    public enum EventType
    {
        Watched,

        Returned,

        Borrowed,
    }

    public sealed class User
    {
        [XmlAttribute]
        public string FirstName;

        [XmlAttribute]
        public string LastName;

        [XmlAttribute]
        public string EmailAddress;

        [XmlAttribute]
        public string PhoneNumber;
    }
}