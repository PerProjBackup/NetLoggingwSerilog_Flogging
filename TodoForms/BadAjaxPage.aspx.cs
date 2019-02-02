using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TodoForms
{
  public partial class BadAjaxPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void UpdateClick(object sender, EventArgs e)
    {
      var convertedInt = Convert.ToInt32(Tb1.Text);
      Tb1.Text = "Completed!";
    }
  }
}