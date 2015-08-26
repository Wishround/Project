using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WishroundProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
    name: "Wish",
    url: "Wish/{id}",
    defaults: new { controller = "Wish", action = "Index", id = UrlParameter.Optional }
);

            routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Wish", action = "Create", id = UrlParameter.Optional }
);



        }
    }
}
