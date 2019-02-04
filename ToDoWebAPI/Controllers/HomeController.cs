using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToDoWebAPI.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Get()
    {
      ViewBag.Title = "Home Page";
      return View();
    }
  }
}
