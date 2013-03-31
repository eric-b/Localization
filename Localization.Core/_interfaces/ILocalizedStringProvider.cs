using System.Globalization;

namespace Localization.Core
{
    /// <summary>
    /// <para>Provider for localized strings.</para>
    /// </summary>
    public interface ILocalizedStringProvider
    {
        /// <summary>
        /// <para>Gets the culture of native strings.</para>
        /// </summary>
        CultureInfo NativeCulture { get; }

        /// <summary>
        /// <para>Returns the translation of a native string.</para>
        /// <para>If the string is unknown, a new record will be created.</para>
        /// </summary>
        /// <param name="source">Native text source (type name, view path, etc.).</param>
        /// <param name="text">Native text.</param>
        /// <param name="targetCulture">Target culture.</param>
        /// <returns></returns>
        string Translate(string source, string text, CultureInfo targetCulture);
    }
}
