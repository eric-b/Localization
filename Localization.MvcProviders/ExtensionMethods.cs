using System;
using System.Globalization;
using System.Web.Mvc;
using Localization.Core;

namespace Localization
{
    /// <summary>
    /// <para>Extension methods to localize strings.</para>
    /// <remarks>This class must not be used before initialization of the <see cref="DependencyResolver"/>. 
    /// <see cref="ITypeNameFactory"/> and <see cref="ILocalizedStringProvider"/> must be registered in the <see cref="DependencyResolver"/>.</remarks>
    /// </summary>
    public static class ExtensionMethods
    {
        private static readonly ITypeNameFactory SourceNameFactory;
        private static readonly ILocalizedStringProvider Localizer;
        private static readonly bool SourceNameFactoryOk, LocalizerOk, DependencyResolverIsReady;

        static ExtensionMethods()
        {
            if (DependencyResolver.Current == null)
                return;
            DependencyResolverIsReady = true;
            SourceNameFactory = DependencyResolver.Current.GetService<ITypeNameFactory>();
            if (SourceNameFactory == null)
                return;
            SourceNameFactoryOk = true;
            Localizer = DependencyResolver.Current.GetService<ILocalizedStringProvider>();
            if (Localizer == null)
                return;
            LocalizerOk = true;

            var logger = DependencyResolver.Current.GetService<ILogger>();
            if (logger != null)
                logger.Debug("Localization: native strings culture: {0}.", Localizer.NativeCulture.DisplayName);
        }

        /// <summary>
        /// Localizes a string.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="text">Native string.</param>
        /// <param name="args">Optional args used with <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>Localized string (empty string if <paramref name="text"/> is <c>null</c> or empty).</returns>
        public static string Translate(this Controller controller, string text, params object[] args)
        {
            return Translate(text, controller, args);
        }

        /// <summary>
        /// Localizes a string.
        /// </summary>
        /// <param name="owner">Caller object (used as source name for the string).</param>
        /// <param name="text">Native string.</param>
        /// <param name="args">Optional args used with <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>Localized string (empty string if <paramref name="text"/> is <c>null</c> or empty).</returns>
        public static string Translate(this string text, object owner, params object[] args)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (!DependencyResolverIsReady)
                throw new InvalidOperationException("DependencyResolver.Current was null at initialization of this static class.");
            if (!SourceNameFactoryOk)
                throw new InvalidOperationException("ITypeNameFactory service cannot be resolved from DependencyResolver.Current.");
            if (!LocalizerOk)
                throw new InvalidOperationException("ILocalizer service cannot be resolved from DependencyResolver.Current.");
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return args.Length != 0
                ?
                string.Format(Localizer.Translate(SourceNameFactory.GetSourceName(owner.GetType()), text, CultureInfo.CurrentUICulture, false), args)
                :
                Localizer.Translate(SourceNameFactory.GetSourceName(owner.GetType()), text, CultureInfo.CurrentUICulture, false);
        }

        
    }
}
