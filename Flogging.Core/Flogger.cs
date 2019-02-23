using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Core
{
  public static class Flogger
  {
    private static readonly ILogger _perfLogger;
    private static readonly ILogger _usageLogger;
    private static readonly ILogger _errorLogger;
    private static readonly ILogger _diagnosticsLogger;
    //private static readonly string path =
    //  @"D:\DevProj\Trng\NetLgngSeriDoneRight\Logs\";

    static Flogger()
    {
      // For SQL Server, add the Serilog.Sinks.MSqlServer Nuget package -
      //    -- also needs to be included in any apps that
      //          directly ref Flogging.Core for writing logs!
      // ASP.NET WebForms, ASP.NET MVC, WebAPI (todos)

      // For Elasticsearch, add Serilog.Sinks.Elasticsearch
      //  update direct ref assemblies shown above.
      // sink details: https://github.com/serilog/serilog-sinks-elasticsearch

      // Sink details: https://github.com/serilog/serilog-sinks-mssqlserver  

      //var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

      _perfLogger = new LoggerConfiguration()
        //.WriteTo.File(path: path + "perf.txt")
        //.WriteTo.MSSqlServer(connStr, "PerfLogs", autoCreateSqlTable: true, 
        //  columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1).CreateLogger(); // default batchPostingLimit: is 50, setting low for development
        .WriteTo.Elasticsearch("http://192.168.99.100:9200",
            indexFormat: "perf-{0:yyyy.MM.dd}", inlineFields: true).CreateLogger();
      _usageLogger = new LoggerConfiguration()
        //.WriteTo.File(path: path + "usage.txt")
        //.WriteTo.MSSqlServer(connStr, "UsageLogs", autoCreateSqlTable: true, 
        //  columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1).CreateLogger();
        .WriteTo.Elasticsearch("http://192.168.99.100:9200",
            indexFormat: "usage-{0:yyyy.MM.dd}", inlineFields: true).CreateLogger();
      _errorLogger = new LoggerConfiguration()
        //.WriteTo.File(path: path + "error.txt")
        //.WriteTo.MSSqlServer(connStr, "ErrorLogs", autoCreateSqlTable: true,
        //  columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1).CreateLogger();
        .WriteTo.Elasticsearch("http://192.168.99.100:9200",
            indexFormat: "error-{0:yyyy.MM.dd}", inlineFields: true).CreateLogger();
      _diagnosticsLogger = new LoggerConfiguration()
        //.WriteTo.File(path: path + "diagnostic.txt")
        //.WriteTo.MSSqlServer(connStr, "DiagnosticLogs", autoCreateSqlTable: true,
        //  columnOptions: GetSqlColumnOptions(), batchPostingLimit: 1).CreateLogger();
        .WriteTo.Elasticsearch("http://192.168.99.100:9200",
            indexFormat: "diagnostic-{0:yyyy.MM.dd}", inlineFields: true).CreateLogger();
    }

    private static ColumnOptions GetSqlColumnOptions()
    {
      var colOptions = new ColumnOptions();
      colOptions.Store.Remove(StandardColumn.Properties);
      colOptions.Store.Remove(StandardColumn.MessageTemplate);
      colOptions.Store.Remove(StandardColumn.Message);
      colOptions.Store.Remove(StandardColumn.Exception);
      colOptions.Store.Remove(StandardColumn.TimeStamp);
      colOptions.Store.Remove(StandardColumn.Level);

      colOptions.AdditionalDataColumns = new Collection<DataColumn> {
        new DataColumn {DataType = typeof(DateTime), ColumnName = "TimeStamp" },
        new DataColumn {DataType = typeof(string), ColumnName = "Product" },
        new DataColumn {DataType = typeof(string), ColumnName = "Layer" },
        new DataColumn {DataType = typeof(string), ColumnName = "Location" },
        new DataColumn {DataType = typeof(string), ColumnName = "Message" },
        new DataColumn {DataType = typeof(string), ColumnName = "Hostname" },
        new DataColumn {DataType = typeof(string), ColumnName = "UserId" },
        new DataColumn {DataType = typeof(string), ColumnName = "UserName" },
        new DataColumn {DataType = typeof(string), ColumnName = "Exception" },
        new DataColumn {DataType = typeof(int), ColumnName = "ElapsedMilliseconds" },
        new DataColumn {DataType = typeof(string), ColumnName = "CorrelationId" },
        new DataColumn {DataType = typeof(string), ColumnName = "CustomException" },
        new DataColumn {DataType = typeof(string), ColumnName = "AdditionalInfo" }    };
      return colOptions;
    }

    public static void WritePerf(FlogDetail infoToLog)
    { _perfLogger.Write(LogEventLevel.Information, // "{@FlogDetail}", infoToLog);
        "{Timestamp}{Message}{Layer}{Location}{Product}" +
        "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
        "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
        infoToLog.Timestamp, infoToLog.Message,
        infoToLog.Layer, infoToLog.Location,
        infoToLog.Product, infoToLog.CustomException,
        infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
        infoToLog.Hostname, infoToLog.UserId,
        infoToLog.UserName, infoToLog.CorrelationId,
        infoToLog.AdditionalInfo ); }

    public static void WriteUsage(FlogDetail infoToLog)
    { _usageLogger.Write(LogEventLevel.Information, // "{@FlogDetail}", infoToLog);
        "{Timestamp}{Message}{Layer}{Location}{Product}" +
        "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
        "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
        infoToLog.Timestamp, infoToLog.Message,
        infoToLog.Layer, infoToLog.Location,
        infoToLog.Product, infoToLog.CustomException,
        infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
        infoToLog.Hostname, infoToLog.UserId,
        infoToLog.UserName, infoToLog.CorrelationId,
        infoToLog.AdditionalInfo ); }


    public static void WriteError(FlogDetail infoToLog)
    {
      if (infoToLog.Exception != null) {
        var procName = FindProcName(infoToLog.Exception);
        infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
        infoToLog.Message = GetMessageFromException(infoToLog.Exception); }
      _errorLogger.Write(LogEventLevel.Information, // "{@FlogDetail}", infoToLog);
        "{Timestamp}{Message}{Layer}{Location}{Product}" +
        "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
        "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
        infoToLog.Timestamp, infoToLog.Message,
        infoToLog.Layer, infoToLog.Location,
        infoToLog.Product, infoToLog.CustomException,
        infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
        infoToLog.Hostname, infoToLog.UserId,
        infoToLog.UserName, infoToLog.CorrelationId,
        infoToLog.AdditionalInfo ); }

    public static void WriteDiagnostic(FlogDetail infoToLog)
    {
      var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
      if (!writeDiagnostics) return;
      _diagnosticsLogger.Write(LogEventLevel.Information, //"{@FlogDetail}", infoToLog); 
        "{Timestamp}{Message}{Layer}{Location}{Product}" +
        "{CustomException}{ElapsedMilliseconds}{Exception}{Hostname}" +
        "{UserId}{UserName}{CorrelationId}{AdditionalInfo}",
        infoToLog.Timestamp, infoToLog.Message,
        infoToLog.Layer, infoToLog.Location,
        infoToLog.Product, infoToLog.CustomException,
        infoToLog.ElapsedMilliseconds, infoToLog.Exception?.ToBetterString(),
        infoToLog.Hostname, infoToLog.UserId,
        infoToLog.UserName, infoToLog.CorrelationId,
        infoToLog.AdditionalInfo ); }

    private static string GetMessageFromException(Exception ex)
    {
      if (ex.InnerException != null) // recursive call through chain of exceptions
        return GetMessageFromException(ex.InnerException);
      return ex.Message;
    }

    private static string FindProcName(Exception ex)
    {
      var sqlEx = ex as SqlException;
      if (sqlEx != null) {
        var procName = sqlEx.Procedure;
        if (!string.IsNullOrEmpty(procName)) return procName;  }
      if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
          return (string)ex.Data["Procedure"];
      if (ex.InnerException != null)
          return FindProcName(ex.InnerException);
      return null;
    }
  }
}
