using System.Text;
using DoenaSoft.MediaInfoHelper.DataObjects;
using DoenaSoft.ToolBox.Generics;
using AbmXml = DoenaSoft.MediaInfoHelper.DataObjects.AudioBookMetaXml;
using NA = global::NAudio.Wave;
using TL = global::TagLib;

namespace DoenaSoft.MediaInfoHelper.Readers;

/// <summary />
public sealed class AudioBookReader
{
    /// <summary />
    public delegate AbmXml.BookRole GetRole(string bookTitle, string person);

    /// <summary />
    public delegate string GetAuthor(string bookTitle);

    /// <summary />
    public delegate string GetNarrator(string bookTitle);

    /// <summary />
    public delegate void Log(string message);

    private readonly GetRole _getRole;

    private readonly GetAuthor _getAuthor;

    private readonly GetNarrator _getNarrator;

    private readonly Log _log;

    /// <summary />
    public AudioBookReader(GetRole getRole = null
        , GetAuthor getAuthor = null
        , GetNarrator getNarrator = null
        , Log log = null)
    {
        _getRole = getRole;
        _getAuthor = getAuthor;
        _getNarrator = getNarrator;
        _log = log;
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
        var meta = this.GetMeta(folder, filePattern);

        if (meta == null)
        {
            return;
        }

        XmlSerializer<AbmXml.AudioBookMeta>.Serialize(metaFileName, meta);
    }

    /// <summary/>
    public AbmXml.AudioBookMeta GetMeta(DirectoryInfo folder, string filePattern = "*.mp3")
    {
        var files = folder
           .GetFiles(filePattern, SearchOption.AllDirectories)
           .OrderBy(fi => fi.FullName)
           .ToList();

        if (files.Count == 0)
        {
            return null;
        }

        var length = this.GetLength(files);

        var meta = this.GetTagMeta(files[0], length);

        return meta;
    }

    private TimeParts GetLength(List<FileInfo> files)
    {
        var totalLength = new TimeSpan(0);

        foreach (var file in files)
        {
            _log?.Invoke($"Processing '{file.Name}'.");

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
        _log?.Invoke($"Processing '{file.Name}'.");

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

    private AbmXml.AudioBookMeta GetTagMeta(FileInfo file, TimeParts length)
    {
        using (var fileMeta = TL.File.Create(file.FullName))
        {
            var tag = fileMeta?.Tag;

            var meta = new AbmXml.AudioBookMeta();

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

            if (_getRole != null || _getAuthor != null || _getNarrator != null)
            {
                this.GetPeople(people, bookName, authors, narrators);
            }

            if (tag != null)
            {
                meta.Title = bookName;
                meta.Author = authors.ToArray();
                meta.Narrator = narrators.ToArray();
                meta.Genre = tag.Genres?.SelectMany(g => Split(g)).ToArray();
                meta.Description = GetDescription(tag);
            }

            meta.RunningTime = new AbmXml.RunningTime()
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
        if (_getRole != null)
        {
            foreach (var person in people)
            {
                var bookRole = _getRole(bookName, person);

                if ((bookRole & AbmXml.BookRole.Author) == AbmXml.BookRole.Author)
                {
                    authors.Add(person);
                }

                if ((bookRole & AbmXml.BookRole.Narrator) == AbmXml.BookRole.Narrator)
                {
                    narrators.Add(person);
                }
            }
        }

        if (authors.Count == 0 && _getAuthor != null)
        {
            var author = _getAuthor(bookName);

            if (!string.IsNullOrWhiteSpace(author))
            {
                authors.Add(author);
            }
        }

        if (narrators.Count == 0 && _getNarrator != null)
        {
            var narrator = _getNarrator(bookName);

            if (!string.IsNullOrWhiteSpace(narrator))
            {
                narrators.Add(narrator);
            }
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
