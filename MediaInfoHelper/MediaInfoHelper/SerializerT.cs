namespace DoenaSoft.MediaInfoHelper
{
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using IO = System.IO;

    /// <summary>
    /// Generic Serializer which contains methods to (de)serialize data structures to and from XML.
    /// </summary>
    /// <typeparam name="T">Type of the data structure</typeparam>
    public static class Serializer<T> where T : class, new()
    {
        private static XmlSerializer _serializer;

        private static Encoding DefaultEncoding { get; }

        static Serializer()
        {
            DefaultEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// The XmlSerializer primed for the data structure <typeparamref name="T"/>.
        /// </summary>
        public static XmlSerializer XmlSerializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(T));
                }

                return _serializer;
            }
        }

        /// <summary>
        /// Deserializes the content of <paramref name="fileName"/> into the data structure <typeparamref name="T"/>.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public static T Deserialize(string fileName)
        {
            using (var fs = new IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read))
            {
                return Deserialize(fs);
            }
        }

        /// <summary>
        /// Deserializes the content of <paramref name="textReader"/> the data structure <typeparamref name="T"/>.
        /// </summary>
        /// <param name="textReader">The TextReader</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public static T Deserialize(IO.TextReader textReader)
            => (T)XmlSerializer.Deserialize(textReader);

        /// <summary>
        /// Deserializes the content of <paramref name="stream"/> the data structure <typeparamref name="T"/>.
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public static T Deserialize(IO.Stream stream)
            => (T)XmlSerializer.Deserialize(stream);

        /// <summary>
        /// Serializes an instance of the data structure <typeparamref name="T"/> into a file.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="instance">The data structure</param>
        /// <param name="encoding">The text encoding. Optional; if null then <see cref="Encoding.UTF8" /> is used.</param>
        public static void Serialize(string fileName, T instance, Encoding encoding = null)
        {
            using (var fs = new IO.FileStream(fileName, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Read))
            {
                Serialize(fs, instance, encoding);
            }
        }

        /// <summary>
        /// Serializes an instance of the data structure <typeparamref name="T"/> into a stream.
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <param name="instance">The data structure</param>
        /// <param name="encoding">The text encoding. Optional; if null then <see cref="Encoding.UTF8" /> is used.</param>
        public static void Serialize(IO.Stream stream, T instance, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            using (var xtw = new XmlTextWriter(stream, encoding))
            {
                xtw.Formatting = Formatting.Indented;
                xtw.Namespaces = false;

                Serialize(xtw, instance);
            }
        }

        /// <summary>
        /// Serializes an instance of the data structure <typeparamref name="T"/> into an XmlWriter.
        /// </summary>
        /// <param name="xmlWriter">The XmlWriter</param>
        /// <param name="instance">The data structure</param>
        public static void Serialize(XmlWriter xmlWriter, T instance)
        {
            var ns = new XmlSerializerNamespaces();

            ns.Add(string.Empty, string.Empty);

            XmlSerializer.Serialize(xmlWriter, instance, ns);
        }

        /// <summary>
        /// Deserializes the content of <paramref name="text"/> the data structure <typeparamref name="T"/>.
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="encoding">The text encoding. Optional; if null then <see cref="Encoding.UTF8" /> is used.</param>
        /// <returns>An instance of <typeparamref name="T"/></returns>
        public static T FromString(string text, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            using (var ms = new IO.MemoryStream(encoding.GetBytes(text)))
            {
                var result = Deserialize(ms);

                return result;
            }
        }

        /// <summary>
        /// Serializes an instance of the data structure <typeparamref name="T"/> into a string.
        /// </summary>
        /// <param name="instance">The data structure</param>
        /// <param name="encoding">The text encoding. Optional; if null then <see cref="Encoding.UTF8" /> is used.</param>
        /// <returns></returns>
        public static string ToString(T instance, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            using (var ms = new IO.MemoryStream())
            {
                Serialize(ms, instance, encoding);

                var result = encoding.GetString(ms.ToArray());

                return result;
            }
        }
    }
}