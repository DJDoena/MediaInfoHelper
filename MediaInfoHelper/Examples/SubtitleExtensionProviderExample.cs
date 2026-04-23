using DoenaSoft.MediaInfoHelper.Contracts;
using DoenaSoft.MediaInfoHelper.DefaultProviders;
using DoenaSoft.MediaInfoHelper.Helpers;

namespace MediaInfoHelper.Examples;

/// <summary>
/// Example implementation of a custom subtitle extension provider for web-based subtitle formats.
/// </summary>
public class WebSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    /// <summary>
    /// Gets web-friendly subtitle formats: SRT, VTT, and TTML.
    /// </summary>
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[]
        {
            "srt",   // SubRip - widely supported
            "vtt",   // WebVTT - HTML5 standard
            "ttml",  // Timed Text Markup Language - XML-based
            "sbv"    // YouTube subtitle format
        };
    }
}

/// <summary>
/// Example implementation for professional broadcast subtitle formats.
/// </summary>
public class BroadcastSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    /// <summary>
    /// Gets professional broadcast subtitle formats.
    /// </summary>
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[]
        {
            "stl",   // Spruce STL (EBU)
            "cap",   // Cheetah CAP
            "scc",   // Scenarist Closed Captions
            "ttml",  // Timed Text Markup Language
            "dfxp",  // Distribution Format Exchange Profile
            "srt"    // SubRip - for compatibility
        };
    }
}

/// <summary>
/// Example implementation for DVD/Blu-ray subtitle formats only.
/// </summary>
public class OpticalDiscSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    /// <summary>
    /// Gets DVD and Blu-ray subtitle formats.
    /// </summary>
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[]
        {
            "idx",   // VobSub index
            "sub",   // VobSub subtitle
            "sup",   // Blu-ray PGS
            "ifo"    // DVD IFO
        };
    }
}

/// <summary>
/// Example showing how to configure and use custom subtitle extension providers.
/// </summary>
public static class SubtitleExtensionProviderExample
{
    /// <summary>
    /// Configures the library to use the web subtitle extension provider.
    /// </summary>
    public static void ConfigureForWebContent()
    {
        // Set provider for web-based content
        MediaInfoConfiguration.SubtitleExtensionProvider = new WebSubtitleExtensionProvider();
    }

    /// <summary>
    /// Configures the library to use the broadcast subtitle extension provider.
    /// </summary>
    public static void ConfigureForBroadcast()
    {
        // Set provider for broadcast content
        MediaInfoConfiguration.SubtitleExtensionProvider = new BroadcastSubtitleExtensionProvider();
    }

    /// <summary>
    /// Configures the library to use the optical disc subtitle extension provider.
    /// </summary>
    public static void ConfigureForOpticalDisc()
    {
        // Set provider for DVD/Blu-ray content
        MediaInfoConfiguration.SubtitleExtensionProvider = new OpticalDiscSubtitleExtensionProvider();
    }

    /// <summary>
    /// Resets the subtitle extension provider to the default implementation.
    /// </summary>
    public static void ResetToDefault()
    {
        // Reset to default behavior
        MediaInfoConfiguration.SubtitleExtensionProvider = DefaultSubtitleExtensionProvider.Instance;
    }

    /// <summary>
    /// Demonstrates how to configure and use custom subtitle extension providers.
    /// </summary>
    public static void DemoUsage()
    {
        // Configure for web content
        ConfigureForWebContent();

        var extensions = MediaInfoConfiguration.SubtitleExtensionProvider.GetSubtitleExtensions();
        Console.WriteLine("Web subtitle formats:");
        foreach (var ext in extensions)
        {
            Console.WriteLine($"  - {ext}");
        }

        // Reset when done
        ResetToDefault();
    }
}
