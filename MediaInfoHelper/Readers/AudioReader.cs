using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoenaSoft.MediaInfoHelper.DataObjects;
using DoenaSoft.MediaInfoHelper.DataObjects.Mp3MetaXml;
using DoenaSoft.MediaInfoHelper.Helpers;
using NA = global::NAudio.Wave;
using TL = global::TagLib;

namespace DoenaSoft.MediaInfoHelper.Readers
{
    /// <summary />
    public sealed class AudioReader
    {
        private readonly bool _manualInput;

        private readonly Action<string> _logger;

        private readonly object _lock;

        /// <summary />
        public AudioReader(bool manualInput, Action<string> logger)
        {
            _manualInput = manualInput;
            _logger = logger;

            _lock = new object();
        }

        /// <summary/>
        public TimeParts GetLength(DirectoryInfo folder, string filePattern = "*.mp3")
        {
            var files = folder
                .GetFiles(filePattern, SearchOption.AllDirectories)
                .OrderBy(fi => fi.FullName)
                .ToList();

            var length = this.GetLength(files);

            return length;
        }

        /// <summary/>
        public void CreateXml(DirectoryInfo folder, string metaFileName, string filePattern = "*.mp3")
        {
            var files = folder
               .GetFiles(filePattern, SearchOption.AllDirectories)
               .OrderBy(fi => fi.FullName)
               .ToList();

            if (files.Count == 0)
            {
                return;
            }

            var length = this.GetLength(files);

            var meta = GetTagMeta(files[0], length);

            XmlSerializer<Mp3Meta>.Serialize(metaFileName, meta);
        }

        private TimeParts GetLength(List<FileInfo> files)
        {
            var totalLength = new TimeSpan(0);

            foreach (var file in files)
            {
                _logger($"Processing '{file.Name}'.");

                using (var reader = new NA.MediaFoundationReader(file.FullName))
                {
                    totalLength += reader.TotalTime;
                }
            }

            var length = GetLength(totalLength);

            return length;
        }

        /// <summary/>
        public TimeParts GetLength(FileInfo file)
        {
            _logger($"Processing '{file.Name}'.");

            using (var reader = new NA.MediaFoundationReader(file.FullName))
            {
                var length = GetLength(reader.TotalTime);

                return length;
            }
        }

        private static TimeParts GetLength(TimeSpan totalLength)
        {
            var days = totalLength.Days;

            var hours = totalLength.Hours;

            var minutes = totalLength.Minutes;

            var seconds = totalLength.Seconds;

            if (totalLength.Milliseconds >= 500)
            {
                seconds++;
            }

            if (seconds == 60)
            {
                seconds = 0;

                minutes++;
            }

            if (minutes == 60)
            {
                minutes = 0;

                hours++;
            }

            if (hours == 24)
            {
                hours = 0;

                days++;
            }

            if (days > 0)
            {
                hours += days * 24;
            }

            return new TimeParts((ulong)days, (ushort)hours, (ushort)minutes, (ushort)seconds);
        }

        private Mp3Meta GetTagMeta(FileInfo file, TimeParts length)
        {
            using (var fileMeta = TL.File.Create(file.FullName))
            {
                var tag = fileMeta?.Tag;

                var meta = new Mp3Meta();

#pragma warning disable CS0618 // Type or member is obsolete
                var people = (tag.AlbumArtists?.SelectMany(a => Split(a)) ?? Enumerable.Empty<string>())
                    .Union(tag.Performers?.SelectMany(p => Split(p)) ?? Enumerable.Empty<string>())
                    .Union(tag.Artists?.SelectMany(p => Split(p)) ?? Enumerable.Empty<string>())
                    .ToList();
#pragma warning restore CS0618 // Type or member is obsolete

                var bookName = !string.IsNullOrWhiteSpace(tag.Album)
                    ? tag.Album
                    : file.Directory.Name;

                var authors = new List<string>();

                var narrators = new List<string>();

                if (_manualInput)
                {
                    GetPeople(people, bookName, authors, narrators);
                }

                if (tag != null)
                {
                    meta.Title = bookName;
                    meta.Author = authors.ToArray();
                    meta.Narrator = narrators.ToArray();
                    meta.Genre = tag.Genres?.SelectMany(g => Split(g)).ToArray();
                    meta.Description = GetDescription(tag);
                }

                meta.RunningTime = new RunningTime()
                {
                    Hours = (length.Days * 24) + length.Hours,
                    Minutes = length.Minutes,
                    Seconds = length.Seconds,
                    Value = $"{length.Hours}:{length.Minutes:D2}:{length.Seconds:D2}",
                };

                return meta;
            }
        }

        private void GetPeople(List<string> people, string bookName, List<string> authors, List<string> narrators)
        {
            foreach (var p in people)
            {
                var answer = string.Empty;

                lock (_lock)
                {
                    while (answer != "a" && answer != "n" && answer != "b")
                    {
                        Console.Write($"Is {p} (a)uthor, (n)arrator or (b) for '{bookName}'? ");

                        answer = Console.ReadLine();
                    }
                }

                switch (answer)
                {
                    case "a":
                        {
                            authors.Add(p);

                            break;
                        }
                    case "n":
                        {
                            narrators.Add(p);

                            break;
                        }
                    case "b":
                        {
                            authors.Add(p);

                            narrators.Add(p);

                            break;
                        }
                }
            }

            if (authors.Count == 0)
            {
                var answer = string.Empty;

                lock (_lock)
                {
                    Console.Write($"No author found for '{bookName}'. Please enter author: ");

                    answer = Console.ReadLine();
                }

                authors.Add(answer);
            }

            if (narrators.Count == 0)
            {
                var answer = string.Empty;

                lock (_lock)
                {
                    Console.Write($"No narrator found for '{bookName}'. Please enter narrator: ");

                    answer = Console.ReadLine();
                }

                narrators.Add(answer);
            }
        }

        #region GetDescription

        private static string GetDescription(TL.Tag tag)
        {
            var result = tag is TL.NonContainer.Tag nct
                ? GetDescription(nct)
                : tag.Comment;

            return result?.Trim();
        }

        private static string GetDescription(TL.NonContainer.Tag tag)
        {
            var texts = tag.Tags?
                .Where(t => t != null)
                .OfType<TL.Id3v2.Tag>()
                .SelectMany(t => t.OfType<TL.Id3v2.TextInformationFrame>())
                .ToList() ?? Enumerable.Empty<TL.Id3v2.TextInformationFrame>();

            var title3 = texts.FirstOrDefault(t => t.FrameId == "TIT3");

            if (title3?.Text.Length > 0)
            {
                var sb = new StringBuilder();

                foreach (var text in title3.Text)
                {
                    sb.AppendLine(text);
                }

                return sb.ToString();
            }
            else
            {
                return tag.Comment;
            }
        }

        #endregion

        private static IEnumerable<string> Split(string text)
            => text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());
    }
}
