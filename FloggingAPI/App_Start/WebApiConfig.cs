using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FloggingAPI
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      var cors = new EnableCorsAttribute("http://localhost:3725, http://localhost:4200", "*", "*");
      config.EnableCors(cors);

      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "logging/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}
