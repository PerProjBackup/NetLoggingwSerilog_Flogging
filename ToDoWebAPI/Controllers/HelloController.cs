using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ToDoWebAPI.Controllers
{
  [AllowAnonymous]
  public class HelloController : ApiController
  {
    public IEnumerable<string> Get()
    {
      return new string[] { "Hello", "World" };
    }
  }
}
