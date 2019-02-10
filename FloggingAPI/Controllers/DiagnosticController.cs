using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FloggingAPI.Controllers
{
  public class DiagnosticController : ApiController
  {
    // Requires EnableDiagnostics=true in web.config Appsettings
    public void Write([FromBody] FlogDetail logEntry)
    {
      Flogger.WriteDiagnostic(logEntry);
    }

  }
}
