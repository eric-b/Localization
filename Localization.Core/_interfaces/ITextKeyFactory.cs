namespace Localization.Core
{
    /// <summary>
    /// <para>Interface responsible of the text key generation.</para>
    /// </summary>
    public interface ITextKeyFactory
    {
        /// <summary>
        /// Returns a unique key based on a native string.
        /// </summary>
        /// <param name="source">Native text source (type name, view path, etc.).</param>
        /// <param name="text">Native string.</param>
        /// <returns></returns>
        string Create(string source, string text);
    }
}
