using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flogging.Web.Attributes
{
  public class TrackUsageAttribute : ActionFilterAttribute
  {
    private string _product;
    private string _layerName;
    private string _name;

    public TrackUsageAttribute(string product, string layer, string name)
    { _product = product; _layerName = layer; _name = name; }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      Helpers.LogWebUsage(_product, _layerName, _name);
    }
  }
}
