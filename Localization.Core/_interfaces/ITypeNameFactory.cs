using System;

namespace Localization.Core
{
    /// <summary>
    /// <para>Interface responsible of the source name generation based on a type.</para>
    /// </summary>
    public interface ITypeNameFactory
    {
        /// <summary>
        /// Returns the source name based on a specific type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetSourceName(Type type);
    }
}
