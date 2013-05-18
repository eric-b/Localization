using System.Globalization;

namespace Localization.Core
{
    /// <summary>
    /// Extension point to treat the case of missing localized string.
    /// </summary>
    public interface IMissingLocalizedStringExtensionPoint
    {
        /// <summary>
        /// <para>This method is called when a localized string was not found in the target culture.</para>
        /// <para>The <paramref name="text"/> may be in default (ie fallback) culture or in native culture.</para>
        /// </summary>
        /// <param name="text">String in default or native culture (see <paramref name="textCulture"/>).</param>
        /// <param name="targetCulture">Target (ie current) culture for which localized string is missing.</param>
        /// <param name="textCulture">Culture of <paramref name="text"/>.</param>
        /// <returns>Should return <paramref name="text"/> possibly formatted in some special way to bring out missing localization.</returns>
        string ProcessMissingLocalizedString(string text, CultureInfo targetCulture, CultureInfo textCulture);
    }
}
