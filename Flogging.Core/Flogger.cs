using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Core
{
  public static class Flogger
  {
    private static readonly ILogger _perfLogger;
    private static readonly ILogger _useageLogger;
    private static readonly ILogger _errorLogger;
    private static readonly ILogger _diagnosticsLogger;

    static Flogger()
    {
      var path = @"D:\DevProj\Trng\NetLgngSeriDoneRight\Logs\";
      _perfLogger = new LoggerConfiguration()
        .WriteTo.File(path: path + "perf.txt").CreateLogger();
      _useageLogger = new LoggerConfiguration()
        .WriteTo.File(path: path + "usage.txt").CreateLogger();
      _errorLogger = new LoggerConfiguration()
        .WriteTo.File(path: path + "error.txt").CreateLogger();
      _diagnosticsLogger = new LoggerConfiguration()
        .WriteTo.File(path: path + "diagnostic.txt").CreateLogger();
    }

    public static void WritePerf(FlogDetail infoToLog)
    { _perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog); }
    public static void WriteUseage(FlogDetail infoToLog)
    { _useageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog); }
    public static void WriteError(FlogDetail infoToLog)
    { _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog); }
    public static void WriteDiagnostic(FlogDetail infoToLog)
    {
      var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
      if (!writeDiagnostics) return;
      _diagnosticsLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog); }
  }
}
