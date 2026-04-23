# MediaInfoHelper

A .NET library to query audio and video files for their metadata information.

## Description

DoenaSoft.MediaInfoHelper is a comprehensive library that provides easy access to metadata from various media file formats including video files and audio books. It wraps popular media information libraries and provides a unified interface for extracting technical details and metadata from media files.

## Features

- Read video file metadata (duration, resolution, codec, etc.)
- Extract audio book information (chapters, duration, tags)
- Support for multiple media formats
- Built on top of proven libraries:
  - FFProbe (via NReco.VideoInfo)
  - NAudio for audio file analysis
  - TagLib for tag reading
- Simple and intuitive API
- Strongly-typed data objects for media information
- **Customizable language standardization and weighting**
- **Configurable subtitle file format detection**

## Installation

Install via NuGet Package Manager:

```
Install-Package DoenaSoft.MediaInfoHelper
```

Or via .NET CLI:

```
dotnet add package DoenaSoft.MediaInfoHelper
```

## Requirements

- .NET Framework 4.7.2 or higher

## Usage

### Reading Video File Information

```csharp
using DoenaSoft.MediaInfoHelper.DataObjects;
using DoenaSoft.MediaInfoHelper.Readers;

// Create a media file object
var mediaFile = new MediaFile { FileName = @"C:\path\to\video.mp4" };

// Create a video reader
var videoReader = new VideoReader(mediaFile);

// Determine the length of the video
videoReader.DetermineLength();

// Access the length
uint durationInSeconds = mediaFile.Length;
```

### Reading Audio Book Information

```csharp
using DoenaSoft.MediaInfoHelper.Readers;

// Create an audio book reader
var audioBookReader = new AudioBookReader();

// Get total length of audio files in a directory
var directoryInfo = new DirectoryInfo(@"C:\path\to\audiobook");
var totalLength = audioBookReader.GetLength(directoryInfo);

Console.WriteLine($"Hours: {totalLength.Hours}");
Console.WriteLine($"Minutes: {totalLength.Minutes}");
Console.WriteLine($"Seconds: {totalLength.Seconds}");
```

### Customizing Language Handling

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

// Implement your own language provider
public class CustomLanguageProvider : ILanguageProvider
{
    public string StandardizeLanguage(string language)
    {
        // Your custom language standardization logic
        return language?.ToLower();
    }

    public int GetLanguageWeight(string language)
    {
        // Your custom language priority (lower = higher priority)
        return 100;
    }
}

// Configure at application startup
MediaInfoConfiguration.LanguageProvider = new CustomLanguageProvider();
```

### Customizing Subtitle Format Detection

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

// Implement your own subtitle extension provider
public class CustomSubtitleProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        // Return the subtitle formats you want to support
        return new[] { "srt", "vtt", "ass" };
    }
}

// Configure at application startup
MediaInfoConfiguration.SubtitleExtensionProvider = new CustomSubtitleProvider();
```

## Customization

### Language Provider

The library includes a default language provider that standardizes common language codes:
- German (de, deu, ger -> de)
- English (en, eng -> en)
- Arabic (ar, ara -> ar)
- Spanish (es, spa -> es)
- Japanese (ja, jap, jpn -> ja)
- Korean (ko, kor -> ko)

You can implement `ILanguageProvider` to define your own language handling logic.

### Subtitle Extension Provider

The library includes a default subtitle extension provider that supports:
- srt (SubRip Text)
- sub (MicroDVD / Sub Station Alpha)
- sup (Blu-ray PGS)
- idx (VobSub index)
- ass (Advanced SubStation Alpha)
- ssa (Sub Station Alpha)
- vtt (WebVTT)

You can implement `ISubtitleExtensionProvider` to customize which subtitle formats are detected.

For detailed documentation on customization, see:
- LANGUAGE_CUSTOMIZATION.md
- SUBTITLE_CUSTOMIZATION.md
- LANGUAGE_PROVIDER_QUICKREF.md

## Data Objects

The library provides strongly-typed classes for different types of media:

- `MediaFile` - General media file information
- `VideoMeta` - Video-specific metadata
- `AudioBookMeta` - Audio book metadata including chapters and roles
- `FFProbeMeta` - Raw FFProbe output data
- `TimeParts` - Time duration broken into hours, minutes, seconds

## Dependencies

- DoenaSoft.ToolBox
- NReco.VideoInfo (FFProbe wrapper)
- NAudio (Audio file processing)
- TagLib (Metadata tag reading)

## License

This project is licensed under the MIT License.

## Author

DJ Doena
Doena Soft.

## Repository

https://github.com/DJDoena/MediaInfoHelper

## Support

For issues, questions, or contributions, please visit the GitHub repository.
