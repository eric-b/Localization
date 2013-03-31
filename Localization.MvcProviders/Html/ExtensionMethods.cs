using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Localization.Core;

namespace Localization.MvcProviders.Html
{
    /// <summary>
    /// <para>Extension methods to localize strings from a view, via <see cref="HtmlHelper"/>.</para>
    /// <remarks>This class must not be used before initialization of the <see cref="DependencyResolver"/>. 
    /// <see cref="IViewNameFactory"/> and <see cref="ILocalizedStringProvider"/> must be registered in the <see cref="DependencyResolver"/>.</remarks>
    /// </summary>
    public static class ExtensionMethods
    {
       
        private static readonly IViewNameFactory SourceNameFactory;
        private static readonly ILocalizedStringProvider Localizer;
        private static readonly bool ViewNameFactoryOk, LocalizerOk, DependencyResolverIsReady;

        static ExtensionMethods()
        {
            if (DependencyResolver.Current == null)
                return;
            DependencyResolverIsReady = true;
            SourceNameFactory = DependencyResolver.Current.GetService<IViewNameFactory>();
            if (SourceNameFactory == null)
                return;
            ViewNameFactoryOk = true;
            Localizer = DependencyResolver.Current.GetService<ILocalizedStringProvider>();
            if (Localizer == null)
                return;
            LocalizerOk = true;

            var logger = DependencyResolver.Current.GetService<ILogger>();
            if (logger != null)
                logger.Debug("MvcLocalization.Core.Html: native views culture: {0}.", Localizer.NativeCulture.DisplayName);
        }

        /// <summary>
        /// Localizes a string.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">Native string.</param>
        /// <param name="args">Optional args used with <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>Localized string (empty string if <paramref name="text"/> is <c>null</c> or empty).</returns>
        public static IHtmlString Translate(this HtmlHelper helper
            , string text, params object[] args)
        {
            if (!DependencyResolverIsReady)
                throw new InvalidOperationException("DependencyResolver.Current was null at initialization of this static class.");
            if (!ViewNameFactoryOk)
                throw new InvalidOperationException("IViewNameFactory service cannot be resolved from DependencyResolver.Current.");
            if (!LocalizerOk)
                throw new InvalidOperationException("ILocalizer service cannot be resolved from DependencyResolver.Current.");
            if (string.IsNullOrWhiteSpace(text))
                return new HtmlString(string.Empty);

            return new HtmlString(args.Length != 0
                ?
                string.Format(Localizer.Translate(SourceNameFactory.GetSourceName(helper.ViewContext.RouteData), text, CultureInfo.CurrentUICulture), args)
                :
                Localizer.Translate(SourceNameFactory.GetSourceName(helper.ViewContext.RouteData), text, CultureInfo.CurrentUICulture)
                );
        }
    }

}
