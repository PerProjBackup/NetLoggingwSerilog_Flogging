using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ToDoWebAPI.Controllers
{
  public class SecureController : ApiController
  {
    // GET api/values
    public string Get(string goodOrBad)
    {
      if (goodOrBad.ToLower() != "good")
        throw new Exception("Not the droids you're looking for.");

      return "Hello there!";
    }


  }
}
