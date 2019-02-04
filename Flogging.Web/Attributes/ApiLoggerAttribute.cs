using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http.Controllers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using Flogging.Core;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace Flogging.Web.Attributes
{
  public class ApiLoggerAttribute : ActionFilterAttribute
  {
    private string _productName;

    public ApiLoggerAttribute(string productName)
    { _productName = productName; }

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      var dict = new Dictionary<string, object>();

      string userId, userName, location; // 
      var user = HttpContext.Current.User as ClaimsPrincipal;   // actionContext.RequestContext.Principal

      Helpers.GetUserData(dict, user, out userId, out userName);
      Helpers.GetLocationForApiCall(actionContext.RequestContext, dict, out location);

      //var qs = HttpContext.Current.Request.QueryString; // actionContext.Request.Get
      //var qs = actionContext.Request.GetQueryNameValuePairs()
      //  .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
      //var i = 0;
      //foreach (var q in qs)
      //{
      //  var newKey = string.Format($"q-{i++}-{q.Key}");
      //  if (!dict.ContainsKey(newKey)) dict.Add(newKey, q.Value);
      //}

      //var referrer = actionContext.Request.Headers.Referrer;
      //if (referrer != null)
      //{
      //  var source = actionContext.Request.Headers.Referrer.OriginalString;
      //  if (source.ToLower().Contains("swagger")) source = "Swagger";
      //  if (!dict.ContainsKey("Referrer")) dict.Add("Referrer", source);
      //}

      //filterContext.Request.Properties
      actionContext.Request.Properties["PerfTracker"] = new PerfTracker(location,
          userId, userName, location, _productName, "API", dict);
    }

    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    {
      try {
        var perfTracker = actionExecutedContext.Request
          .Properties["PerfTracker"] as PerfTracker;
        if (perfTracker != null) perfTracker.Stop();
      } catch (Exception) {
        // ignoring logging exceptions -- don't want an API call to fail
        // if we run into logging problems.
      }
    }
  }
}
