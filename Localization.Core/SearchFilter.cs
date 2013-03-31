using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Localization.Core
{
    [DataContract]
    public class SearchFilter : ISearchFilter
    {
        public readonly static SearchFilter Empty = new SearchFilter();

        [DataMember]
        public bool OnlyNotTranslated { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string Reserved { get; set; }

        #region IEquatable

        public bool Equals(ISearchFilter other)
        {
            return other != null && other.GetHashCode() == this.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as SearchFilter);
        }

        public override int GetHashCode()
        {
            return string.Format("{0}|{1}|{2}", OnlyNotTranslated, Source, Reserved).GetHashCode();
        }

        #endregion
    }
}
