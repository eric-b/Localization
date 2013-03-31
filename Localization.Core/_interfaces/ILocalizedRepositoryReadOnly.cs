using System.Collections.Generic;
using System.Globalization;
using Localization.Core.Entity;

namespace Localization.Core
{
    /// <summary>
    /// Provides read-only access to the localized strings repository.
    /// </summary>
    public interface ILocalizedRepositoryReadOnly
    {
        /// <summary>
        /// Returns a localized string identified by its key.
        /// </summary>
        /// <param name="culture">Target culture</param>
        /// <param name="key">Text key</param>
        /// <returns></returns>
        LocalizedString GetString(CultureInfo culture, string key);

        /// <summary>
        /// Returns localized strings for the specified culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<LocalizedString> GetStrings(CultureInfo culture, ISearchFilter filter);

        /// <summary>
        /// Returns existing cultures.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CultureInfo> GetCultures();

        /// <summary>
        /// Returns <c>true</c> if culture exists.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        bool Exists(CultureInfo culture);
    }
}
