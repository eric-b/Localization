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

        private readonly CultureInfo _nativeCulture;

        /// <summary>
        /// <para>Gets the culture of native strings.</para>
        /// </summary>
        public CultureInfo NativeCulture
        {
            get { return _nativeCulture; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="textKeyFactory"></param>
        /// <param name="logger"></param>
        /// <param name="nativeCulture">Culture of native strings.</param>
        public DefaultLocalizedStringProvider(ILocalizedRepository repository, ITextKeyFactory textKeyFactory, ILogger logger, CultureInfo nativeCulture)
        {
            _repository = repository;
            _textKeyFactory = textKeyFactory;
            _logger = logger;
            _nativeCulture = nativeCulture;
        }

        public string Translate(string source, string text, CultureInfo targetCulture)
        {
            var key = _textKeyFactory.Create(source, text);
            var prompt = _repository.GetString(targetCulture, key);
            if (prompt == null)
            {
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
                }
                return FormatMissingTranslation(text, targetCulture);
            }

            return string.IsNullOrEmpty(prompt.TranslatedText) ? FormatMissingTranslation(text, targetCulture) : prompt.TranslatedText;
        }

        
        private string FormatMissingTranslation(string text, CultureInfo targetCulture)
        {
            return _nativeCulture.Name == targetCulture.Name ? text : string.Format("[{1}: {0}]", text, targetCulture.Name);
        }
    }
}
