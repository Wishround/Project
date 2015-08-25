using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WishroundProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ProductsApi",
                routeTemplate: "api/{controller}/{action}/{orderId}",
                defaults: new { orderId = RouteParameter.Optional }
            );

        }
    }
}
