using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Localization.Core.Entity;

namespace Localization
{
    /// <summary>
    /// Very basic XML repository for demonstration purposes.
    /// </summary>
    public sealed class XmlFileRepository : Core.ILocalizedRepository
    {
        private readonly Dictionary<Tuple<string, int>, LocalizedString> _strings;
        private readonly string _filepath;

        private const string RootAttribute = "LocalizedStrings";
        private const string Namespace = "http://github.com/eric-b/Localization/";

        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<LocalizedString>), null, null,
                                                             new XmlRootAttribute(RootAttribute),
                                                             Namespace);

        public XmlFileRepository(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new  ArgumentNullException("filepath");
            _filepath = filepath;

            if (File.Exists(filepath))
            {
                List<LocalizedString> strings;
                using (var reader = new StreamReader(filepath, Encoding.UTF8))
                {
                    strings = (List<LocalizedString>)_serializer.Deserialize(reader);
                }
                _strings = new Dictionary<Tuple<string, int>, LocalizedString>((strings.Count));
                var groups = strings.GroupBy(t => new Tuple<string, int>(t.Key, CultureInfo.GetCultureInfo(t.Culture).LCID));
                foreach (var group in groups)
                {
                    _strings.Add(group.Key, group.First());
                }
            }
            else
            {
                _strings = new Dictionary<Tuple<string, int>, LocalizedString>();
            }
        }

        public void CreateCulture(CultureInfo culture)
        {
            
        }

        private void Save()
        {
            using (var writer = new StreamWriter(_filepath))
            {
                _serializer.Serialize(writer, _strings.Values.ToList());
            }
        }

        public void Save(CultureInfo culture, string textKey, string source, string text, string translatedText)
        {
            var value = new LocalizedString()
                {
                    Culture =  culture.Name,
                    Key = textKey,
                    Source = source,
                    Text = text,
                    TranslatedText = translatedText
                };
            var key = new Tuple<string, int>(textKey, culture.LCID);
            _strings.Remove(key);
            _strings.Add(key, value);
            Save();
        }

        public void Delete(CultureInfo culture, string textKey)
        {
            _strings.Remove(new Tuple<string, int>(textKey, culture.LCID));
            Save();
        }

        public LocalizedString GetString(CultureInfo culture, string key)
        {
            LocalizedString value;
            return _strings.TryGetValue(new Tuple<string, int>(key, culture.LCID), out value) ? value : null;
        }

        public IEnumerable<LocalizedString> GetStrings(CultureInfo culture, Core.ISearchFilter filter)
        {
            var cultureName = culture.Name;
            return _strings.Values.Where(t =>
                {
                    if (t.Culture != cultureName)
                        return false;

                    if (filter.OnlyNotTranslated && !string.IsNullOrEmpty(t.TranslatedText))
                        return false;

                    if (!string.IsNullOrEmpty(filter.Source) && t.Source != filter.Source)
                        return false;

                    return true;
                });
        }

        public IEnumerable<CultureInfo> GetCultures()
        {
            return _strings.Keys.Select(t => t.Item2).Distinct().Select(CultureInfo.GetCultureInfo);
        }

        public bool Exists(CultureInfo culture)
        {
            return _strings.Any(t => t.Value.Culture == culture.Name);
        }
    }
}
