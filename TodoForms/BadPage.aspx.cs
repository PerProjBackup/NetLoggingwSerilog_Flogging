using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TodoForms
{
    public partial class BadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ex = new Exception("Something bad happened!!!");
            ex.Data.Add("keyinfo", "No input params specified");
            throw ex;
        }
    }
}