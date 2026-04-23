using DoenaSoft.MediaInfoHelper.Contracts;

namespace DoenaSoft.MediaInfoHelper.DefaultProviders;

/// <summary>
/// Default implementation of <see cref="ISubtitleExtensionProvider"/> that supports common subtitle formats.
/// </summary>
public sealed class DefaultSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    /// <summary>
    /// Gets the singleton instance of the default subtitle extension provider.
    /// </summary>
    public static DefaultSubtitleExtensionProvider Instance { get; }

    static DefaultSubtitleExtensionProvider()
    {
        Instance = new DefaultSubtitleExtensionProvider();
    }

    private DefaultSubtitleExtensionProvider()
    {
    }

    /// <summary>
    /// Gets the default subtitle extensions: srt, sub, sup, idx, ass, ssa, vtt.
    /// </summary>
    /// <returns>Collection of subtitle file extensions.</returns>
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return
        [
            "srt",  // SubRip Text
            "sub",  // MicroDVD / Sub Station Alpha
            "sup",  // Blu-ray subtitle format
            "idx",  // VobSub index
            "ass",  // Advanced SubStation Alpha
            "ssa",  // Sub Station Alpha
            "vtt"   // WebVTT
        ];
    }
}
