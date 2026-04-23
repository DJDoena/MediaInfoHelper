# Language & Subtitle Provider Quick Reference

## Quick Start - Language Provider

### Using Default Behavior (No Changes)
```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

// Just use as before - no configuration needed
var standardized = LanguageExtensions.StandardizeLanguage("deu");  // "de"
```

### Custom Language Provider (3 Steps)

#### Step 1: Implement ILanguageProvider
```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

public class MyLanguageProvider : ILanguageProvider
{
    public string StandardizeLanguage(string language)
    {
        // Your standardization logic
        return language?.ToLower();
    }

    public int GetLanguageWeight(string language)
    {
        // Your weighting logic (lower = higher priority)
        return 100;
    }
}
```

#### Step 2: Configure (Once at Startup)
```csharp
MediaInfoConfiguration.LanguageProvider = new MyLanguageProvider();
```

#### Step 3: Use Normally
```csharp
// All methods automatically use your provider
var result = LanguageExtensions.StandardizeLanguage("french");
var weight = LanguageExtensions.GetLanguageWeight("fr");

var distinctLangs = audioTracks.GetDistinctLanguages();
var standardized = audioTracks.StandardizeLanguage();
```

## Quick Start - Subtitle Extension Provider

### Using Default Behavior (No Changes)
```csharp
// Default supports: srt, sub, sup, idx, ass, ssa, vtt
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var additionalSubtitles);
```

### Custom Subtitle Extension Provider (3 Steps)

#### Step 1: Implement ISubtitleExtensionProvider
```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

public class MySubtitleExtensionProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[] { "srt", "vtt", "ttml" };
    }
}
```

#### Step 2: Configure (Once at Startup)
```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = new MySubtitleExtensionProvider();
```

#### Step 3: Use Normally
```csharp
// FFProbeReader will search for your specified subtitle formats
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var additionalSubtitles);
```

## API Reference

### ILanguageProvider
```csharp
string StandardizeLanguage(string language);  // Normalize language codes
int GetLanguageWeight(string language);       // Get sort priority (lower=higher)
```

### ISubtitleExtensionProvider
```csharp
IEnumerable<string> GetSubtitleExtensions();  // Get subtitle file extensions
```

### MediaInfoConfiguration
```csharp
static ILanguageProvider LanguageProvider { get; set; }                      // Global language provider
static ISubtitleExtensionProvider SubtitleExtensionProvider { get; set; }    // Global subtitle provider
```

### DefaultLanguageProvider
```csharp
static DefaultLanguageProvider Instance { get; }  // Singleton default provider
```

### DefaultSubtitleExtensionProvider
```csharp
static DefaultSubtitleExtensionProvider Instance { get; }  // Singleton default provider
```

## Default Language Mappings

| Input | Output | Weight |
|-------|--------|--------|
| de, deu, ger | de | 1 |
| en, eng | en | 2 |
| es, spa | es | 3 |
| ar, ara | ar | 4 |
| ja, jap, jpn | ja | 5 |
| ko, kor | ko | 6 |
| others | lowercase | 10+ |

## Default Subtitle Extensions

| Extension | Format |
|-----------|--------|
| srt | SubRip Text |
| sub | MicroDVD / Sub Station Alpha |
| sup | Blu-ray PGS |
| idx | VobSub index |
| ass | Advanced SubStation Alpha |
| ssa | Sub Station Alpha |
| vtt | WebVTT |

## Examples

See:
- `MediaInfoHelper\Examples\EuropeanLanguageProviderExample.cs` - Language provider example
- `MediaInfoHelper\Examples\SubtitleExtensionProviderExample.cs` - Subtitle provider examples
- `MediaInfoHelper.Tests\LanguageProviderTests.cs` - Language provider tests
- `MediaInfoHelper.Tests\SubtitleExtensionProviderTests.cs` - Subtitle provider tests
- `LANGUAGE_CUSTOMIZATION.md` - Detailed language customization guide
- `SUBTITLE_CUSTOMIZATION.md` - Detailed subtitle customization guide
