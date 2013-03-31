using System.Web.Routing;

namespace Localization.MvcProviders
{
    /// <summary>
    /// <para>Interface responsible of the source name generation based on a view.</para>
    /// </summary>
    public interface IViewNameFactory
    {
        /// <summary>
        /// Returns the source name based on a specific view.
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        string GetSourceName(RouteData routeData);
    }
}
