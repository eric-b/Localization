using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Localization.Core.Entity
{
    /// <summary>
    /// <para>Localized string entity.</para>
    /// <para>Each string is identified by its <see cref="Key"/> (same key for all versions).</para>
    /// </summary>
    [DataContract]
    public class LocalizedString : IEquatable<LocalizedString>
    {
        /// <summary>
        /// Identifies the culture (<see cref="CultureInfo.Name"/>).
        /// </summary>
        [DataMember]
        public string Culture { get; set; }

        /// <summary>
        /// Identifies the string (same key for all versions).
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Identifies the source of <see cref="Text"/>.
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Native string.
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Translated version.
        /// </summary>
        [DataMember]
        public string TranslatedText { get; set; }

        #region IEquatable
        public bool Equals(LocalizedString other)
        {
            return (other != null) && other.Key.Equals(Key);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LocalizedString);
        }

        public override int GetHashCode()
        {
            return Key != null ? Key.GetHashCode() : 0;
        }
        #endregion
    }
}
