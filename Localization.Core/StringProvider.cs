using System;
using System.Globalization;

namespace Localization.Core
{
    /// <summary>
    /// <para>Localized strings provider.</para>
    /// <para>This provider should be used from a legacy class (not a view nor a model...).</para>
    /// </summary>
    /// <typeparam name="TOwner">Identifies the owner class.</typeparam>
    public class StringProvider<TOwner> where TOwner : class
    {
        private readonly string _sourceName;

        private readonly ILocalizedStringProvider _localizer;

        public StringProvider(ITypeNameFactory sourceNameFactory, ILocalizedStringProvider localizer)
        {
            _sourceName = sourceNameFactory.GetSourceName(typeof(TOwner));
            if (string.IsNullOrEmpty(_sourceName))
                throw new InvalidOperationException(string.Format("{0} returned an empty string for the type {1}.", sourceNameFactory.GetType().Name, typeof(TOwner).FullName));
            _localizer = localizer;
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="textName">Native string</param>
        /// <param name="formatterArgs">Arguments used with <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>Localized string</returns>
        public string Translate(string textName, params object[] formatterArgs)
        {
            return
            formatterArgs.Length == 0
                ? _localizer.Translate(_sourceName, textName, CultureInfo.CurrentUICulture, false)
                : string.Format(_localizer.Translate(_sourceName, textName, CultureInfo.CurrentUICulture, false), formatterArgs);
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="preventMissingLocalizedStringBehavior">Prevents use of <see cref="IMissingLocalizedStringExtensionPoint"/>
        /// if localized string in target culture is missing. The value should be <c>false</c> in most cases.</param>
        /// <param name="textName">Native string</param>
        /// <param name="formatterArgs">Arguments used with <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>Localized string</returns>
        public string Translate(bool preventMissingLocalizedStringBehavior, string textName, params object[] formatterArgs)
        {
            return
            formatterArgs.Length == 0
                ? _localizer.Translate(_sourceName, textName, CultureInfo.CurrentUICulture, preventMissingLocalizedStringBehavior)
                : string.Format(_localizer.Translate(_sourceName, textName, CultureInfo.CurrentUICulture, preventMissingLocalizedStringBehavior), formatterArgs);
        }
    }
}
