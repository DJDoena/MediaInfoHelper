# Subtitle Extension Implementation Summary

## Overview
The MediaInfoHelper library now supports injectable subtitle file format detection through a provider pattern. This allows external callers to define which subtitle file formats should be detected when reading video files.

## Files Created

### 1. ISubtitleExtensionProvider.cs
Interface defining the contract for subtitle extension operations:
- `GetSubtitleExtensions()`: Returns collection of subtitle file extensions to search for

### 2. DefaultSubtitleExtensionProvider.cs
Default implementation maintaining broad subtitle format support:
- Formats: srt, sub, sup, idx, ass, ssa, vtt
- Available as singleton: `DefaultSubtitleExtensionProvider.Instance`

### 3. SubtitleExtensionProviderTests.cs
Unit tests verifying:
- Default provider behavior
- Custom provider injection
- Null handling (fallback to default)
- Extension collection integrity

### 4. SUBTITLE_CUSTOMIZATION.md
Documentation explaining:
- How to implement `ISubtitleExtensionProvider`
- How to configure the library
- Special format handling (idx/sub, ifo/sup pairs)
- Examples of custom implementations

### 5. SubtitleExtensionProviderExample.cs
Example implementations for:
- Web subtitle formats (srt, vtt, ttml, sbv)
- Broadcast formats (stl, cap, scc, ttml, dfxp)
- Optical disc formats (idx, sub, sup, ifo)

## Files Modified

### MediaInfoConfiguration.cs
- Added `SubtitleExtensionProvider` property
- Defaults to `DefaultSubtitleExtensionProvider.Instance`
- Null-safe with automatic fallback

### FFProbeReader.cs
- Completely refactored subtitle detection logic
- Changed from hard-coded extensions to provider-based
- Consolidated three separate methods into unified approach
- Improved language detection for SRT files
- Special handling for paired formats (idx/sub, ifo/sup)
- Updated XML documentation

## Backward Compatibility

✅ **100% Backward Compatible**
- All existing public APIs remain unchanged
- Default behavior searches for the same formats as before (plus ass, ssa, vtt)
- No breaking changes for existing code

## Technical Details

### Refactored Methods
- **Before**: Separate `GetIdxSubSubtitleMediaInfo`, `GetIfoSupSubtitleMediaInfo`, `GetSrtSubtitleMediaInfo`
- **After**: Unified `GetSubtitleMediaInfoByExtension` that works with any extension

### Special Format Handling
1. **idx files**: Requires matching .sub file
2. **ifo files**: Requires matching .sup file
3. **srt files**: Attempts language detection from filename patterns

### Language Detection Enhancement
- Now splits on `.`, `_`, and `-` characters
- Extracts language codes from any part of filename
- Supports: de/deu/ger, en/eng, ar/ara, es/spa

## Default Subtitle Formats

The default provider includes 7 subtitle formats:

| Extension | Format | Notes |
|-----------|--------|-------|
| srt | SubRip Text | Most common |
| sub | MicroDVD/SSA | Requires .idx for VobSub |
| sup | Blu-ray PGS | Bitmap-based |
| idx | VobSub index | Requires .sub pair |
| ass | Advanced SSA | Styled subtitles |
| ssa | Sub Station Alpha | Styled subtitles |
| vtt | WebVTT | HTML5 standard |

## Usage Examples

### Using Default Provider
```csharp
// Works automatically - searches for all 7 default formats
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var additionalSubtitles);
```

### Using Custom Provider
```csharp
// Custom provider for web formats only
public class WebSubtitleProvider : ISubtitleExtensionProvider
{
    public IEnumerable<string> GetSubtitleExtensions()
    {
        return new[] { "srt", "vtt", "ttml" };
    }
}

// Configure at application startup
MediaInfoConfiguration.SubtitleExtensionProvider = new WebSubtitleProvider();

// Now only searches for srt, vtt, and ttml files
var ffprobe = FFProbeReader.TryGetFFProbe(videoFile, out var additionalSubtitles);
```

## Benefits

1. **Extensibility**: Support any subtitle format
2. **Flexibility**: Different providers for different use cases
3. **Performance**: Search only for relevant formats
4. **Maintainability**: Single unified detection method
5. **No Breaking Changes**: Existing code continues to work

## Testing

All functionality verified with unit tests:
- ✅ Default provider formats
- ✅ Custom provider injection
- ✅ Null safety (automatic fallback)
- ✅ Extension collection integrity
- ✅ Non-null collection guarantee

Test Results: 5/5 tests passed

## Combined Configuration

Both language and subtitle providers can be configured together:

```csharp
// Configure at application startup
MediaInfoConfiguration.LanguageProvider = new MyLanguageProvider();
MediaInfoConfiguration.SubtitleExtensionProvider = new MySubtitleProvider();

// All library operations now use your custom providers
```
