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
