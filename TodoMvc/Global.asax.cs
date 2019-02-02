using Flogging.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TodoMvc.Controllers;

namespace TodoMvc
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      var ex = Server.GetLastError();
      if (ex == null) return;

      int httpStatus;
      string errorControllerAction;

      Helpers.GetHttpStatus(ex, out httpStatus);

      switch (httpStatus)
      {
        case 404: errorControllerAction = "NotFound"; break;
        default:
          Helpers.LogWebError(Constants.ProductName, Constants.LayerName, ex);
          errorControllerAction = "Index"; break;
      }

      var httpContext = ((MvcApplication)sender).Context;
      httpContext.ClearError();
      httpContext.Response.Clear();
      httpContext.Response.StatusCode = httpStatus;
      httpContext.Response.TrySkipIisCustomErrors = true;

      var routeData = new RouteData();
      routeData.Values["controller"] = "Error";
      routeData.Values["action"] = errorControllerAction;

      var controller = new ErrorController();
      ((IController)controller)
        .Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
    }
  }
}
