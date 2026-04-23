# Subtitle Extension Customization

## Overview

The MediaInfoHelper library now supports customizable subtitle file format detection through the `ISubtitleExtensionProvider` interface. This allows you to define which subtitle file formats should be detected when reading video files.

## Default Behavior

By default, the library uses `DefaultSubtitleExtensionProvider` which searches for the following subtitle formats:
- **srt** - SubRip Text
- **sub** - MicroDVD / Sub Station Alpha
- **sup** - Blu-ray subtitle format
- **idx** - VobSub index (requires matching .sub file)
- **ass** - Advanced SubStation Alpha
- **ssa** - Sub Station Alpha
- **vtt** - WebVTT

## Customizing Subtitle Format Detection

### Step 1: Implement ISubtitleExtensionProvider

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

public class CustomSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[]
        {
            "srt",   // SubRip
            "vtt",   // WebVTT
            "ttml",  // Timed Text Markup Language
            "dfxp",  // Distribution Format Exchange Profile
            "sbv",   // YouTube captions
            "stl"    // Spruce subtitle format
        };
    }
}
```

### Step 2: Configure the Library

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

// Set your custom subtitle extension provider globally
MediaInfoConfiguration.SubtitleExtensionProvider = new CustomSubtitleExtensionProvider();

// Now subtitle file detection will use your custom formats
```

### Step 3: Use as Normal

Once configured, the FFProbeReader will automatically search for your specified subtitle formats:

```csharp
using DoenaSoft.MediaInfoHelper.Reader;

// This will now search for .srt, .vtt, .ttml, .dfxp, .sbv, and .stl files
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var additionalSubtitles);
```

## Special Format Handling

The library includes special handling for certain subtitle formats:

### IDX/SUB Pairs
- When searching for `.idx` files, the library checks for a matching `.sub` file
- Both files must exist for the subtitle to be detected

### IFO/SUP Pairs
- When searching for `.ifo` files, the library checks for a matching `.sup` file
- Both files must exist for the subtitle to be detected

### SRT Language Detection
- For `.srt` files, the library attempts to detect the language from the filename
- Supports patterns like: `movie.de.srt`, `movie_eng.srt`, `movie-spa.srt`
- Detected languages: de/deu/ger, en/eng, ar/ara, es/spa

## Extension Format

- Return extensions **without** the leading dot (e.g., `"srt"` not `".srt"`)
- Extensions are case-insensitive during file search
- The order of extensions affects search performance but not results

## Thread Safety

The `MediaInfoConfiguration.SubtitleExtensionProvider` property is static and should be set once at application startup. Setting it multiple times from different threads is not recommended.

## Resetting to Default

To reset to the default subtitle extension provider:

```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = DefaultSubtitleExtensionProvider.Instance;
```

Or simply:

```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = null;  // Automatically falls back to default
```

## Example: Minimal Subtitle Support

If you only care about SRT and VTT subtitles:

```csharp
public class MinimalSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[] { "srt", "vtt" };
    }
}

// Configure at startup
MediaInfoConfiguration.SubtitleExtensionProvider = new MinimalSubtitleExtensionProvider();
```

## Example: Extended Subtitle Support

For maximum subtitle format compatibility:

```csharp
public class ExtendedSubtitleExtensionProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[]
        {
            // Common formats
            "srt", "vtt", "ass", "ssa",

            // DVD/Blu-ray formats
            "sub", "sup", "idx", "ifo",

            // Professional formats
            "stl", "ttml", "dfxp", "scc",

            // Web/streaming formats
            "sbv", "json", "cap"
        };
    }
}
```
