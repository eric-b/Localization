using System;
using System.Globalization;
using System.Linq;

namespace Localization.Core
{
    /// <summary>
    /// Default implementation of <see cref="ILocalizedStringProvider"/>.
    /// </summary>
    public class DefaultLocalizedStringProvider : ILocalizedStringProvider
    {
        private readonly ILocalizedRepository _repository;

        private readonly ITextKeyFactory _textKeyFactory;

        private readonly ILogger _logger;

        /// <summary>
        /// Native culture of strings, used in last resort if localized string is missing (both current culture and fallback culture).
        /// </summary>
        private readonly CultureInfo _nativeCulture;

        /// <summary>
        /// Fallback culture used if localized string is missing.
        /// </summary>
        private readonly CultureInfo _defaultCulture;

        private readonly IMissingLocalizedStringExtensionPoint _missingLocalizedStringExtensionPoint;

        /// <summary>
        /// <para>Gets the culture of native strings.</para>
        /// </summary>
        public CultureInfo NativeCulture
        {
            get { return _nativeCulture; }
        }

        /// <summary>
        /// <para>Gets the fallback culture used if localized string in target (ie current) culture is missing.</para>
        /// <remarks>Can be different of <see cref="NativeCulture"/>. Native culture is used
        /// if both localized string in target culture and default culture are missing.</remarks>
        /// </summary>
        public CultureInfo DefaultCulture
        {
            get { return _defaultCulture; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="textKeyFactory"></param>
        /// <param name="logger"></param>
        /// <param name="nativeCulture">Culture of native strings.</param>
        /// <param name="defaultCulture">Default culture to use if localized string is missing in current culture. If <c>null</c>, <paramref name="nativeCulture"/> is used.</param>
        /// <param name="missingLocalizedStringExtensionPoint">Extension point to treat missing localized string in target culture. If <c>null</c>, an instance of <see cref="DefaultMissingLocalizedStringExtensionPoint"/> is used.</param>
        public DefaultLocalizedStringProvider(ILocalizedRepository repository, ITextKeyFactory textKeyFactory, ILogger logger, CultureInfo nativeCulture, CultureInfo defaultCulture, IMissingLocalizedStringExtensionPoint missingLocalizedStringExtensionPoint)
        {
            _repository = repository;
            _textKeyFactory = textKeyFactory;
            _logger = logger;
            _nativeCulture = nativeCulture;
            _defaultCulture = defaultCulture ?? _nativeCulture;
            _missingLocalizedStringExtensionPoint = missingLocalizedStringExtensionPoint ?? DefaultMissingLocalizedStringExtensionPoint.Instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="textKeyFactory"></param>
        /// <param name="logger"></param>
        /// <param name="nativeCulture">Culture of native strings.</param>
        [Obsolete("Use the most complete ctor.")]
        public DefaultLocalizedStringProvider(ILocalizedRepository repository, ITextKeyFactory textKeyFactory,
                                              ILogger logger, CultureInfo nativeCulture)
            : this(repository, textKeyFactory, logger, nativeCulture, null, null)
        {
        }


        public string Translate(string source, string text, CultureInfo targetCulture, bool preventMissingLocalizedStringBehavior)
        {
            var key = _textKeyFactory.Create(source, text);
            var prompt = _repository.GetString(targetCulture, key);
            if (prompt == null)
            {
                #region String not found in repo
                var languages = _repository.GetCultures();
                if (languages.Any()
                    && !languages.Contains(targetCulture))
                {
                    // Clones strings
                    _logger.Debug("Creates culture {0} ({1})...", targetCulture.Name, source);
                    var strings = _repository.GetStrings(_nativeCulture, SearchFilter.Empty).ToArray();
                    for (int i = 0; i < strings.Length; i++)
                    {
                        var s = strings[i];
                        _repository.Save(targetCulture, s.Key, s.Source, s.Text, string.Empty);   
                    }
                }
                else
                {
                    _logger.Debug("Creates translated string: {0}, {1}, {2}", targetCulture.Name, source, text);
                    _repository.Save(targetCulture, key, source, text, string.Empty);

                    if (_nativeCulture.Equals(targetCulture))
                        return text;
                }

                if (!_nativeCulture.Equals(_defaultCulture))
                {
                    // Tries to find a localized string in default culture
                    prompt = _repository.GetString(_defaultCulture, key);
                    if (prompt != null && !string.IsNullOrEmpty(prompt.TranslatedText))
                        return preventMissingLocalizedStringBehavior ? prompt.TranslatedText : _missingLocalizedStringExtensionPoint.ProcessMissingLocalizedString(prompt.TranslatedText, targetCulture, _defaultCulture);
                }

                // Last resort: returns native string
                return preventMissingLocalizedStringBehavior ? text : _missingLocalizedStringExtensionPoint.ProcessMissingLocalizedString(text, targetCulture, _nativeCulture);
                #endregion
            }

            if (string.IsNullOrEmpty(prompt.TranslatedText))
            {
                if (!_nativeCulture.Equals(targetCulture))
                {
                    // Tries to find localized string in default culture
                    prompt = _repository.GetString(_defaultCulture, key);
                    if (prompt != null && !string.IsNullOrEmpty(prompt.TranslatedText))
                        return preventMissingLocalizedStringBehavior ? prompt.TranslatedText : _missingLocalizedStringExtensionPoint.ProcessMissingLocalizedString(prompt.TranslatedText, targetCulture, _defaultCulture);
                }

                // Last resort: returns native string
                return preventMissingLocalizedStringBehavior ? text : _missingLocalizedStringExtensionPoint.ProcessMissingLocalizedString(text, targetCulture, _nativeCulture);
            }

            // Nominal case: localized string exists
            return prompt.TranslatedText;
        }

        public string Translate(string source, string text, CultureInfo targetCulture)
        {
            return Translate(source, text, targetCulture, false);
        }   
    }
}
