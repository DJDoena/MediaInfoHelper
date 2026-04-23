using DoenaSoft.MediaInfoHelper.Contracts;
using DoenaSoft.MediaInfoHelper.DefaultProviders;

namespace DoenaSoft.MediaInfoHelper.Helpers;

/// <summary>
/// Configuration class for MediaInfoHelper library settings.
/// </summary>
public static class MediaInfoConfiguration
{
    private static ILanguageProvider _languageProvider;

    private static ISubtitleExtensionProvider _subtitleExtensionProvider;

    /// <summary>
    /// Gets or sets the language provider used for language standardization and weighting.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="DefaultLanguageProvider.Instance"/>.
    /// Set this to your own implementation to customize language handling.
    /// </remarks>
    public static ILanguageProvider LanguageProvider
    {
        get => _languageProvider ?? DefaultLanguageProvider.Instance;
        set => _languageProvider = value;
    }

    /// <summary>
    /// Gets or sets the subtitle extension provider used to determine which subtitle file formats to search for.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="DefaultSubtitleExtensionProvider.Instance"/>.
    /// Set this to your own implementation to customize subtitle file detection.
    /// </remarks>
    public static ISubtitleExtensionProvider SubtitleExtensionProvider
    {
        get => _subtitleExtensionProvider ?? DefaultSubtitleExtensionProvider.Instance;
        set => _subtitleExtensionProvider = value;
    }
}
