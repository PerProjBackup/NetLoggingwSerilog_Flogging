using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Flogging.Web.Services
{
  public class CustomApiExceptionHandler : ExceptionHandler
  {
    public override bool ShouldHandle(ExceptionHandlerContext context)
    {
      return true;
    }

    public override void Handle(ExceptionHandlerContext context)
    {
      var errorInfo = string.Empty;

      // this is et within the custom "logger" whcih is called BEFORE this
      // in the exception pipeline
      if (context.Exception.Data.Contains("ErrorId"))
            errorInfo = "Error ID: " + context.Exception.Data["ErrorId"];

      context.Result = new TextPlainErrorResult {
        Request = context.ExceptionContext.Request,
        Content = "Oops! Sorry! Something went wrong.  " +
                  "Please contact our support team so we can try to fix it. " +
                  errorInfo };

    }

    private class TextPlainErrorResult : IHttpActionResult
    {
      public HttpRequestMessage Request{ get; set; }
      public string Content { get; set; }

      public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
      {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError) {
          Content = new StringContent(Content), RequestMessage = Request };

        var referrer = Request.Headers.Referrer;
        if (referrer != null) response.Headers
            // .Add("Access-Control-Allow-Origin", referrer.ToString());
            .Add("Access-Contro-Allow-Origin", referrer.GetLeftPart(UriPartial.Authority));

        return Task.FromResult(response);
      }
    }

  }
}
