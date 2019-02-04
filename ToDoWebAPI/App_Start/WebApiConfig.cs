using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Flogging.Web.Attributes;
using Flogging.Web.Services;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace ToDoWebAPI
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services
      // Configure Web API to use only bearer token authentication.
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
      config.Filters.Add(new AuthorizeAttribute());

      config.Filters.Add(new ApiLoggerAttribute("ToDos"));

      config.Services.Replace(typeof(IExceptionHandler), new CustomApiExceptionHandler());
      config.Services.Add(typeof(IExceptionLogger), new CustomApiExceptionLogger("ToDos"));
      
      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );
    }
  }
}
