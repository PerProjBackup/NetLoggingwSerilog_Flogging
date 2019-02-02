using Flogging.Web.Attributes;
using System.Web;
using System.Web.Mvc;

namespace TodoMvc
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
      filters.Add(new TrackPerformanceAttribute(Constants.ProductName,
            Constants.LayerName));
    }
  }
}
