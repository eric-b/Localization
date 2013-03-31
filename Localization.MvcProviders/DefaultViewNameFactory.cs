namespace Localization.MvcProviders
{
    public class DefaultViewNameFactory : IViewNameFactory
    {
        public string GetSourceName(System.Web.Routing.RouteData routeData)
        {
            const string areaField = "area", controllerField = "Controller", actionField = "Action";
            var area = routeData.Values[areaField] ?? routeData.DataTokens[areaField];
            return area != null
                       ? string.Format("/{0}/{1}/{2}", area, routeData.GetRequiredString(controllerField), routeData.GetRequiredString(actionField))
                       : string.Format("/{0}/{1}", routeData.GetRequiredString(controllerField), routeData.GetRequiredString(actionField));
        }
    }
}
