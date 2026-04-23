namespace DoenaSoft.MediaInfoHelper.Contracts;

/// <summary>
/// Interface for providing language standardization and weighting functionality.
/// </summary>
public interface ILanguageProvider
{
    /// <summary>
    /// Standardizes a language code or name to a consistent format.
    /// </summary>
    /// <param name="language">The language code or name to standardize.</param>
    /// <returns>The standardized language code.</returns>
    string StandardizeLanguage(string language);

    /// <summary>
    /// Gets the sorting weight for a language. Lower values indicate higher priority.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <returns>The weight value for sorting.</returns>
    int GetLanguageWeight(string language);
}