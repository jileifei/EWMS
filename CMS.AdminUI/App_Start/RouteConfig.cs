using System.Web.Mvc;
using System.Web.Routing;

namespace CMS.AdminUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "Paging",
               "{controller}/{action}/{status}/{messageType}/{pageIndex}",
               new { controller = "FeedBack", action = "Index", status = 0, messageType = 0, pageIndex = UrlParameter.Optional }); 
        }
    }
}
