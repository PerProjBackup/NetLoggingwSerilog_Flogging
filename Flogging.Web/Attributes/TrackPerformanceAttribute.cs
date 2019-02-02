using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flogging.Web.Attributes
{
  public class TrackPerformanceAttribute : ActionFilterAttribute
  {
    private string _productName;      private string _layerName;

    // can use like [TrackPerformance("Todos", "Mvc")]
    public TrackPerformanceAttribute(string product, string layer)
    { _productName = product; _layerName = layer; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      string userId, userName, location;
      var dict = Helpers.GetWebFloggingData(out userId, out userName, out location);

      var type = filterContext.HttpContext.Request.RequestType;
      var perfName = filterContext.ActionDescriptor.ActionName + "_" + type;

      var sw = new PerfTracker(perfName, userId, userName, location,
            _productName, _layerName, dict);
      filterContext.HttpContext.Items["Stopwatch"] = sw;
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      var sw = (PerfTracker)filterContext.HttpContext.Items["Stopwatch"];
      if (sw != null) sw.Stop();
    }

  }
}
