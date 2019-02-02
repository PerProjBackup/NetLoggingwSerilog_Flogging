using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Web
{
  public abstract class CustomPage : System.Web.UI.Page
  {
    protected PerfTracker Tracker;

    protected override void OnLoad(EventArgs e)
    {
      var name = Page.Request.Path + (IsPostBack ? "_Postback" : "");

      string userId, userName, Location;
      var data = Helpers.GetWebFloggingData(out userId, out userName, out Location);

      Tracker = new PerfTracker(name, userId, userName, Location, "ToDos", "WebForms", data);
      base.OnLoad(e);
    }

    protected override void OnUnload(EventArgs e)
    {
      base.OnUnload(e);
      if (Tracker != null) Tracker.Stop();
    }
  }
}
