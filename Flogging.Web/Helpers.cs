﻿using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace Flogging.Web
{
  public static class Helpers
  {
    public static void LogWebUsage(string product, string layer, string activityName,
        Dictionary<string, object> addtionalInfo = null )
    {
      string userId, userName, location;
      var webInfo = GetWebFloggingData(out userId, out userName, out location);

      if (addtionalInfo != null)
          foreach (var key in addtionalInfo.Keys)
              webInfo.Add($"Info-{key}", addtionalInfo[key]);

      var usageInfo = new FlogDetail() { Product = product, Layer = layer,
        Location = location, UserId = userId, UserName = userName,
        Hostname = Environment.MachineName,
        CorrelationId = HttpContext.Current.Session.SessionID,
        Message = activityName, AdditionalInfo = webInfo };

      Flogger.WriteUsage(usageInfo);
    }

    public static void LogWebDiagnostic(string product, string layer, string message,
    Dictionary<string, object> diagnosticInfo = null)
    {
      var writeDiagnostics =
        Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
      if (!writeDiagnostics) return;

      string userId, userName, location;
      var webInfo = GetWebFloggingData(out userId, out userName, out location);
      if (diagnosticInfo != null)
          foreach (var key in diagnosticInfo.Keys)
              webInfo.Add(key, diagnosticInfo[key]);

      var diagInfo = new FlogDetail() { Product = product, Layer = layer,
        Location = location, UserId = userId, UserName = userName,
        Hostname = Environment.MachineName,
        CorrelationId = HttpContext.Current.Session.SessionID,
        Message = message, AdditionalInfo = webInfo};

      Flogger.WriteDiagnostic(diagInfo);
    }

    public static void LogWebError(string product, string layer, Exception ex)
    {
      string userId, userName, location;
      var webInfo = GetWebFloggingData(out userId, out userName, out location);
 
      var errorInfo = new FlogDetail() { Product = product, Layer = layer,
        Location = location, UserId = userId, UserName = userName,
        Hostname = Environment.MachineName,
        CorrelationId = HttpContext.Current.Session.SessionID,
        Exception = ex, AdditionalInfo = webInfo };

      Flogger.WriteError(errorInfo);
    }

    public static void GetHttpStatus(Exception ex, out int httpStatus)
    {
      httpStatus = 500;
      if (ex is HttpException)
      {
        var httpEx = ex as HttpException;
        httpStatus = httpEx.GetHttpCode();
      }
    }

    public static Dictionary<string, object> GetWebFloggingData(out string userId,
          out string userName, out string location)
    {
      var data = new Dictionary<string, object>();

      GetRequestData(data, out location);
      GetUserData(data, ClaimsPrincipal.Current, out userId, out userName);
      GetSessionData(data);
      // got cookies?

      return data;
    }

    private static void GetRequestData(Dictionary<string, object> data, out string location)
    {
      location = "";
      var request = HttpContext.Current.Request;
      // rich object - you may want to explore this
      if (request != null)
      {
        location = request.Path;

        string type, version;
        // MS Edge requires special detection logic
        GetBrowserInfo(request, out type, out version);
        data.Add("Browser", $"{type}{version}");
        data.Add("UserAgent", request.UserAgent);
        data.Add("Languages", request.UserLanguages); // non en-US preferences here??
        foreach (var qskey in request.QueryString.Keys) {
          data.Add(string.Format($"QuerySting-{qskey}"),
              request.QueryString[qskey.ToString()]); }
      }
    }

    private static void GetBrowserInfo(HttpRequest request, out string type, out string version)
    {
      type = request.Browser.Type;
      if (type.StartsWith("Chrome") && request.UserAgent.Contains("Edge/")) {
        type = "Edge";
        version = " (v " + request.UserAgent
          .Substring(request.UserAgent.IndexOf("Edge/") + 5) + ")";
      } else {
        version = " (v " + request.Browser.MajorVersion + "." +
          request.Browser.MinorVersion + ")";       }
    }

    internal static void GetUserData(Dictionary<string, object> data, ClaimsPrincipal user, out string userId, out string userName)
    {
      userId = userName = "";
      //var user = ClaimsPrincipal.Current;
      if (user != null) {
        var i = 1; // i included in dictionary key to ensure uniqueness
        foreach (var claim in user.Claims) {
          if (claim.Type == ClaimTypes.NameIdentifier) userId = claim.Value;
          else if (claim.Type == ClaimTypes.Name) userName = claim.Value;
          else // example dictionary key: UserClaim-4-role
            data.Add(string.Format($"UserClaim-{i++}-{claim.Type}"), claim.Value); } }
    }

    internal static void GetLocationForApiCall(HttpRequestContext requestContext,
          Dictionary<string, object> dict, out string location)
    {
      // example route template: api/{controoler}/{id}
      var routeTemplate = requestContext.RouteData.Route.RouteTemplate;

      var method = requestContext.Url.Request.Method; // GET, POST, etc.

      foreach (var key in requestContext.RouteData.Values.Keys)
      {
        var value = requestContext.RouteData.Values[key].ToString();
        if (Int64.TryParse(value, out long numeric)) // C# 7 inline declaration
        { // must be numeric part of route
                dict.Add($"Route-{key}", value);
        } else routeTemplate = routeTemplate.Replace("{" + key + "}", value);
      }

      location = $"{method} {routeTemplate}";

      var qs = HttpUtility.ParseQueryString(requestContext.Url.Request.RequestUri.Query);
      var i = 0;
      foreach (string key in qs.Keys)
      {
        var newKey = string.Format($"q-{i++}-{key}");
        if (!dict.ContainsKey(newKey)) dict.Add(newKey, qs[key]);
      }

      var referrer = requestContext.Url.Request.Headers.Referrer;
      if (referrer != null)
      {
        var source = referrer.OriginalString;
        if (source.ToLower().Contains("swagger")) source = "Swagger";
        if (!dict.ContainsKey("Referrer")) dict.Add("Referrer", source);
      }
    }

    private static void GetSessionData(Dictionary<string, object> data)
    {
      if (HttpContext.Current.Session != null) {
        foreach (var key in HttpContext.Current.Session.Keys) {
          var keyName = key.ToString();
          if (HttpContext.Current.Session[keyName] != null)
            data.Add(string.Format($"Session-{keyName}"),
              HttpContext.Current.Session[keyName].ToString()); } }
      data.Add("SessionId", HttpContext.Current.Session.SessionID);
    }
  }
}
