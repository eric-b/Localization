﻿using System.Globalization;

namespace Localization.Core
{
    /// <summary>
    /// <para>Localized strings manager.</para>
    /// </summary>
    public interface ILocalizedRepository : ILocalizedRepositoryReadOnly
    {
        /// <summary>
        /// Clone strings for a specific culture.
        /// </summary>
        /// <param name="culture">Culture to create.</param>
        void CreateCulture(CultureInfo culture);

        /// <summary>
        /// Add or save a string.
        /// </summary>
        /// <param name="culture">Culture of <paramref name="translatedText"/>.</param>
        /// <param name="textKey">Native text key (generated by <see cref="ITextKeyFactory"/>).</param>
        /// <param name="source">Identifies source of text (type, view...).</param>
        /// <param name="text">Native text.</param>
        /// <param name="translatedText">Translated text.</param>
        void Save(CultureInfo culture, string textKey, string source, string text, string translatedText);

        /// <summary>
        /// Delete a localized string.
        /// </summary>
        /// <param name="culture">Culture of localized string to delete.</param>
        /// <param name="textKey">Native text key.</param>
        void Delete(CultureInfo culture, string textKey);
    }
}
