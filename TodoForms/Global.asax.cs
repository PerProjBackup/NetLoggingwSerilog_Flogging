using Flogging.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TodoForms
{
  public class Global : HttpApplication
  {
    void Application_Start(object sender, EventArgs e)
    {
      // Code that runs on application startup
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      var application = sender as HttpApplication;
      if (application == null) return;

      var ex = Server.GetLastError();
      if (ex == null) return;

      int httpStatus;
      Helpers.GetHttpStatus(ex, out httpStatus);
      if (httpStatus == 404)
      {
        Response.Redirect("~/NotFound.aspx");
      }
      else
      {
        Helpers.LogWebError("ToDos", "WebForms", ex);

        if (!IsAjaxRequest())
        {
          Server.ClearError();
          Response.Redirect("~/TechnicalError.aspx");
          Context.ApplicationInstance.CompleteRequest(); 
        }
      }
    }
    
    public bool IsAjaxRequest()
    {
      var context = HttpContext.Current;
      var isCallbackRequest = false; // callback requests are ajax requests
      if (context != null && context.CurrentHandler != null
                          && context.CurrentHandler is System.Web.UI.Page)
          isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
      return isCallbackRequest || (Request["X-Requested-With"] == "XMLHttpRequest") ||
        (Request.Headers["X-Requested-With"] == "XMLHttpRequest");


    }
  }
}