using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TodoMvc.Controllers
{
    public class BadNewsController : Controller
    {        
        public ActionResult Index()
        {
            throw new Exception("Can't seem to get it right....");            
        }

        public ActionResult AjaxAction()
        {
            return View();
        }

        // for ajax: add Microsoft.jQuery.Unobtrusive.Ajax Nuget package and 
        // script ref somewhere for ~/Scripts/jquery.unobtrusive-ajax.min.js
        public JsonResult DoAjaxStuff(string inputText)
        {
            var myInt = Convert.ToInt32(inputText);
            return Json("Success!");
        }        
    }
}