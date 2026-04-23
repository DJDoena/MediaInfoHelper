# Language Provider Customization

## Overview

The MediaInfoHelper library now supports customizable language standardization and weighting through the `ILanguageProvider` interface. This allows you to define your own language handling logic instead of being limited to the built-in language support.

## Default Behavior

By default, the library uses `DefaultLanguageProvider` which standardizes the following languages:
- German (de, deu, ger -> de)
- English (en, eng -> en)
- Arabic (ar, ara -> ar)
- Spanish (es, spa -> es)
- Japanese (ja, jap, jpn -> ja)
- Korean (ko, kor -> ko)

The default weighting prioritizes languages in this order: German (1), English (2), Spanish (3), Arabic (4), Japanese (5), Korean (6), others (10+).

## Customizing Language Handling

### Step 1: Implement ILanguageProvider

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

public class CustomLanguageProvider : ILanguageProvider
{
    public string StandardizeLanguage(string language)
    {
        // Your custom logic here
        switch (language?.ToLowerInvariant())
        {
            case "fr":
            case "fra":
            case "fre":
                return "fr";
            case "it":
            case "ita":
                return "it";
            // Add more languages as needed
            default:
                return language?.ToLower();
        }
    }

    public int GetLanguageWeight(string language)
    {
        // Your custom weighting logic
        switch (language?.ToLower())
        {
            case "fr":
                return 1;  // French has highest priority
            case "en":
                return 2;
            case "it":
                return 3;
            default:
                return 100 + Math.Abs(language?.GetHashCode() ?? 0);
        }
    }
}
```

### Step 2: Configure the Library

```csharp
using DoenaSoft.MediaInfoHelper.Helpers;

// Set your custom language provider globally
MediaInfoConfiguration.LanguageProvider = new CustomLanguageProvider();

// Now all language operations will use your custom provider
var standardized = LanguageExtensions.StandardizeLanguage("fra");  // Returns "fr"
var weight = LanguageExtensions.GetLanguageWeight("fr");  // Returns 1
```

### Step 3: Use as Normal

Once configured, all existing methods will automatically use your custom provider:

```csharp
using DoenaSoft.MediaInfoHelper.DataObjects.VideoMetaXml;
using DoenaSoft.MediaInfoHelper.Helpers;

// These methods now use your custom language provider
var distinctLanguages = audioTracks.GetDistinctLanguages();
var standardizedAudio = audioTracks.StandardizeLanguage();
var standardizedSubtitles = subtitles.StandardizeLanguage();
```

## Thread Safety

The `MediaInfoConfiguration.LanguageProvider` property is static and should be set once at application startup. Setting it multiple times from different threads is not recommended.

## Resetting to Default

To reset to the default language provider:

```csharp
MediaInfoConfiguration.LanguageProvider = DefaultLanguageProvider.Instance;
```

Or simply:

```csharp
MediaInfoConfiguration.LanguageProvider = null;  // Automatically falls back to default
```
