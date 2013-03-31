using System;

namespace Localization.Core
{
    /// <summary>
    /// Interface used to search localized strings (<see cref="ILocalizedRepository.GetStrings"/>).
    /// </summary>
    public interface ISearchFilter : IEquatable<ISearchFilter>
    {
        /// <summary>
        /// Search for not yet translated strings only.
        /// </summary>
        bool OnlyNotTranslated { get; set; }

        /// <summary>
        /// Search for one specific source (type, view...).
        /// </summary>
        string Source { get; set; }
       
        /// <summary>
        /// <para>Application & concrete repository specific value.</para>
        /// <para>Enable custom filter.</para>
        /// </summary>
        string Reserved { get; set; }
    }
}
