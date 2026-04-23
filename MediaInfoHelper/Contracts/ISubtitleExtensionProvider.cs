namespace DoenaSoft.MediaInfoHelper.Contracts;

/// <summary>
/// Interface for providing subtitle file extensions to search for when reading video files.
/// </summary>
public interface ISubtitleExtensionProvider
{
    /// <summary>
    /// Gets the collection of subtitle file extensions to search for (without leading dot).
    /// </summary>
    /// <returns>Collection of file extensions like "srt", "sub", "sup", etc.</returns>
    IEnumerable<string> GetSubtitleExtensions();
}