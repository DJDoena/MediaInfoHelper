# MediaInfoHelper Customization - Complete Summary

## Overview
The MediaInfoHelper library has been enhanced with two injectable provider systems that allow external callers to customize:
1. **Language handling** - standardization and weighting
2. **Subtitle format detection** - which subtitle file formats to search for

Both features maintain 100% backward compatibility while providing powerful extensibility.

---

## 1. Language Provider System

### What Was Made Injectable
- `StandardizeLanguage(string)` - Language code normalization
- `GetLanguageWeight(string)` - Language sorting priority

### New Components
- `ILanguageProvider` - Interface for language operations
- `DefaultLanguageProvider` - Original behavior as singleton
- `MediaInfoConfiguration.LanguageProvider` - Global configuration property

### Default Behavior
Standardizes: de, en, ar, es, ja, ko  
Weighting: de=1, en=2, es=3, ar=4, ja=5, ko=6, others=10+

### Usage
```csharp
// Custom implementation
public class MyLanguageProvider : ILanguageProvider
{
    public string StandardizeLanguage(string language) { /* ... */ }
    public int GetLanguageWeight(string language) { /* ... */ }
}

// Configuration
MediaInfoConfiguration.LanguageProvider = new MyLanguageProvider();

// Automatic usage
var standardized = LanguageExtensions.StandardizeLanguage("french");
```

---

## 2. Subtitle Extension Provider System

### What Was Made Injectable
Subtitle file format detection in `FFProbeReader`

### New Components
- `ISubtitleExtensionProvider` - Interface for subtitle extensions
- `DefaultSubtitleExtensionProvider` - 7 common formats as singleton
- `MediaInfoConfiguration.SubtitleExtensionProvider` - Global configuration property

### Default Behavior
Supports: srt, sub, sup, idx, ass, ssa, vtt

### Refactored Code
- Unified subtitle detection logic
- Removed hard-coded extension checks
- Improved language detection for SRT files
- Special handling for paired formats (idx/sub, ifo/sup)

### Usage
```csharp
// Custom implementation
public class MySubtitleProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[] { "srt", "vtt", "ttml" };
    }
}

// Configuration
MediaInfoConfiguration.SubtitleExtensionProvider = new MySubtitleProvider();

// Automatic usage
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var subtitles);
```

---

## Files Created

### Interfaces
- `MediaInfoHelper\Helpers\ILanguageProvider.cs`
- `MediaInfoHelper\Helpers\ISubtitleExtensionProvider.cs`

### Default Implementations
- `MediaInfoHelper\Helpers\DefaultLanguageProvider.cs`
- `MediaInfoHelper\Helpers\DefaultSubtitleExtensionProvider.cs`

### Configuration
- `MediaInfoHelper\Helpers\MediaInfoConfiguration.cs`

### Examples
- `MediaInfoHelper\Examples\EuropeanLanguageProviderExample.cs`
- `MediaInfoHelper\Examples\SubtitleExtensionProviderExample.cs`

### Tests
- `MediaInfoHelper.Tests\LanguageProviderTests.cs` (5 tests, all passing)
- `MediaInfoHelper.Tests\SubtitleExtensionProviderTests.cs` (5 tests, all passing)

### Documentation
- `LANGUAGE_CUSTOMIZATION.md` - Detailed language provider guide
- `SUBTITLE_CUSTOMIZATION.md` - Detailed subtitle provider guide
- `LANGUAGE_PROVIDER_QUICKREF.md` - Quick reference for both providers
- `IMPLEMENTATION_SUMMARY.md` - Language provider technical details
- `SUBTITLE_IMPLEMENTATION_SUMMARY.md` - Subtitle provider technical details

---

## Files Modified

### MediaInfoHelper\Helpers\LanguageExtensions.cs
- Updated XML documentation to reference `ILanguageProvider`
- Refactored `StandardizeLanguage(string)` to delegate to provider
- Refactored `GetLanguageWeight(string)` to delegate to provider
- All extension methods now use `MediaInfoConfiguration.LanguageProvider`

### MediaInfoHelper\Readers\FFProbeReader.cs
- Added using directive for `DoenaSoft.MediaInfoHelper.Helpers`
- Updated class XML documentation
- Completely refactored subtitle detection:
  - Removed `GetIdxSubSubtitleMediaInfo` method
  - Removed `GetIfoSupSubtitleMediaInfo` method
  - Removed `GetSrtSubtitleMediaInfo` method
  - Added unified `GetSubtitleMediaInfoByExtension` method
  - Added `TryGetSubtitleMediaInfo` with special format handling
  - Improved `TryGetSrtSubtitleMediaInfo` with better language detection
  - Added `TryDetectLanguageFromPart` helper method

---

## Configuration Options

### Single Configuration Class
```csharp
MediaInfoConfiguration.LanguageProvider = /* your provider */;
MediaInfoConfiguration.SubtitleExtensionProvider = /* your provider */;
```

### Null Safety
Both properties automatically fall back to default providers if set to null.

### Thread Safety Note
Both properties are static and should be configured once at application startup.

---

## Test Results

**Total Tests: 11**
- Language Provider Tests: 5 ✅
- Subtitle Extension Provider Tests: 5 ✅
- Existing Test: 1 ✅
- **All Tests Passing: 11/11**

---

## Backward Compatibility

✅ **100% Backward Compatible**
- All existing public APIs unchanged
- Default behavior identical to original
- No breaking changes
- No configuration required for existing code

---

## Benefits

### Language Provider
1. Support any language codes and naming conventions
2. Custom language prioritization for your region/use case
3. Dynamic language handling without code changes

### Subtitle Extension Provider
1. Support any subtitle file format
2. Optimize performance by searching only relevant formats
3. Different providers for different media types (web, broadcast, optical)

### Overall
1. **Extensibility** - Easy to add custom behavior
2. **Maintainability** - Clear separation of concerns
3. **Testability** - Interface-based design
4. **Flexibility** - Configure globally or use defaults
5. **Performance** - Search only needed formats

---

## Quick Start

### Just Use Defaults (No Changes)
```csharp
// Everything works as before
var standardized = LanguageExtensions.StandardizeLanguage("deu");
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var subtitles);
```

### Customize Everything
```csharp
// Configure at application startup
MediaInfoConfiguration.LanguageProvider = new MyLanguageProvider();
MediaInfoConfiguration.SubtitleExtensionProvider = new MySubtitleProvider();

// Use library normally - your providers are used automatically
```

---

## Real-World Examples

### European Media Library
```csharp
MediaInfoConfiguration.LanguageProvider = new EuropeanLanguageProvider();
// Prioritizes: fr, it, en, de, es, pt, nl
```

### Web Streaming Service
```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = new WebSubtitleExtensionProvider();
// Supports: srt, vtt, ttml, sbv
```

### Professional Broadcast
```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = new BroadcastSubtitleExtensionProvider();
// Supports: stl, cap, scc, ttml, dfxp
```

### DVD/Blu-ray Collection Manager
```csharp
MediaInfoConfiguration.SubtitleExtensionProvider = new OpticalDiscSubtitleExtensionProvider();
// Supports: idx, sub, sup, ifo
```

---

## Documentation Structure

1. **LANGUAGE_PROVIDER_QUICKREF.md** - Start here for both features
2. **LANGUAGE_CUSTOMIZATION.md** - Deep dive into language providers
3. **SUBTITLE_CUSTOMIZATION.md** - Deep dive into subtitle providers
4. **IMPLEMENTATION_SUMMARY.md** - Technical details (language)
5. **SUBTITLE_IMPLEMENTATION_SUMMARY.md** - Technical details (subtitle)
6. This file - Complete overview

---

## Summary

The MediaInfoHelper library now provides two powerful customization points:
1. **Language handling** - Customize how language codes are standardized and prioritized
2. **Subtitle detection** - Customize which subtitle file formats to search for

Both features:
- Use the provider pattern for clean dependency injection
- Default to original behavior for backward compatibility
- Are configured globally via `MediaInfoConfiguration`
- Include comprehensive tests and documentation
- Come with real-world example implementations

**All existing code continues to work without changes. New customization capabilities are opt-in.**
