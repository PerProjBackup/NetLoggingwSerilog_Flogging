using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace FlogViewer.Controllers
{
    [RoutePrefix("api/errorlogs")]
    public class ErrorLogsController : ApiController
    {
        [Route("machines")]
        public List<string> GetMachines(string connKey)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connKey].ToString();

            using (var db = new SqlConnection(connStr))
            {                
                var machines = db.Query<string>(@"
                                    SELECT DISTINCT ISNULL(Hostname, '') AS Hostname
                                    FROM ErrorLogs 
                                    WHERE TimeStamp > DATEADD(dd, -5, GETDATE()) 
                                    ORDER BY Hostname");
                return machines.ToList();
            }
        }

        [Route("layers")]
        public List<string> GetLayers(string connKey)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connKey].ToString();

            using (var db = new SqlConnection(connStr))
            {
                var machines = db.Query<string>(@"
                                    SELECT DISTINCT ISNULL(Layer, '') AS Layer
                                    FROM ErrorLogs 
                                    WHERE TimeStamp > DATEADD(dd, -5, GETDATE()) 
                                    ORDER BY Layer");
                return machines.ToList();
            }
        }

        [Route("users")]
        public List<string> GetUsers(string connKey)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connKey].ToString();

            using (var db = new SqlConnection(connStr))
            {
                var machines = db.Query<string>(@"
                                    SELECT DISTINCT ISNULL(UserName, '') AS UserName
                                    FROM ErrorLogs 
                                    WHERE TimeStamp > DATEADD(dd, -5, GETDATE()) 
                                    ORDER BY UserName");
                return machines.ToList();
            }
        }
        
        [Route("entries")]
        public List<FlogDetailPoco> GetEntries(string connKey, string machineList, 
                       string layerList, string userList, DateTime beginDate, DateTime? endDate,
                       string like, string notLike, int limitTo, int logId, string correlationId)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connKey].ToString();

            var where = GetWhereCondition(machineList, layerList, userList, beginDate,
                                    endDate, like, notLike, logId, correlationId);

            using (var db = new SqlConnection(connStr))
            {
                var logEntries = db.Query<FlogDetailPoco>(@"
                                    SELECT TOP " + 
                                            (limitTo > 1000 ? "1000" : limitTo.ToString()) + @"   
                                        Id, Timestamp, Product, Layer, Location, 
                                        Message, Hostname, UserId, UserName, 
                                        ElapsedMilliseconds, CorrelationId, Exception, 
                                        CustomException, AdditionalInfo 
                                    FROM ErrorLogs " + 
                                    where + @"
                                    ORDER BY Id DESC");
                foreach (var entry in logEntries)
                {
                    if (!string.IsNullOrEmpty(entry.Exception))
                    {
                        var htmlRx = new Regex("<");
                        entry.Exception = htmlRx.Replace(entry.Exception, "&lt;");
                        htmlRx = new Regex(">");
                        entry.Exception = htmlRx.Replace(entry.Exception, "&gt;");                        
                        htmlRx = new Regex("\\n");
                        entry.Exception = htmlRx.Replace(entry.Exception, "<br/>");
                        var tabRx = new Regex("\\t");
                        entry.Exception = tabRx.Replace(entry.Exception, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

                        var messageRx = new Regex("(Message:)(.*?)<br/>");
                        entry.Exception = messageRx.Replace(entry.Exception, @"$1 <span style=""background-color:yellow;"">$2</span><br/>");
                    }
                    if (!string.IsNullOrEmpty(entry.CustomException))
                    {
                        var htmlRx = new Regex("<");
                        entry.CustomException = htmlRx.Replace(entry.CustomException, "&lt;");
                        htmlRx = new Regex(">");
                        entry.CustomException = htmlRx.Replace(entry.CustomException, "&gt;");
                        htmlRx = new Regex("\\n");
                        entry.CustomException = htmlRx.Replace(entry.CustomException, "<br/>");
                        var tabRx = new Regex("\\t");
                        entry.CustomException = tabRx.Replace(entry.CustomException, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                        var messageRx = new Regex("(.Message:)(.*?)<br/>");
                        entry.CustomException = messageRx.Replace(entry.CustomException, @"$1 <span style=""background-color:yellow;"">$2</span><br/>");
                    }

                }

                return logEntries.ToList();
            }
        }

        private string GetWhereCondition(string machineList, string layerList, string userList, 
            DateTime beginDate, DateTime? endDate, string like, string notLike, int logId, string correlationId)
        {
            if (logId > 0)
                return $"WHERE Id = {logId} ";

            if (!string.IsNullOrEmpty(correlationId))
                return $"WHERE CorrelationId = '{correlationId}'";

            var where = @"
                    WHERE Timestamp > '" + beginDate.ToString("G") + "' ";
            
            if (endDate != null && endDate.Value != DateTime.MinValue)
                where += $" AND Timestamp <= {endDate.Value.ToString("G")}";
            
            if (machineList != "ALL")
            {
                var toAdd = "AND Hostname IN (";
                foreach (var machine in machineList.Split(','))
                {
                    if (toAdd != "AND Hostname IN (")
                        toAdd += ",";
                    toAdd += $"'{machine}'";
                }
                where += toAdd + ") ";
            }
            if (layerList != "ALL")
            {
                var toAdd = "AND Layer IN (";
                foreach (var layer in layerList.Split(','))
                {
                    if (toAdd != "AND Layer IN (")
                        toAdd += ",";
                    toAdd += $"'{layer}'";
                }
                where += toAdd + ") ";
            }

            if (userList != "ALL")
            {
                var toAdd = "AND Username IN (";
                foreach (var user in userList.Split(','))
                {
                    if (toAdd != "AND Username IN (")
                        toAdd += ",";
                    toAdd += $"'{user}'";
                }
                where += toAdd + ") ";
            }

            if (!string.IsNullOrEmpty(like))
                where += $" AND (Message LIKE '%{like}%' " +
                    $"OR Exception LIKE '%{like}%' " +
                    $"OR CustomException LIKE '%{like}%') ";

            if (!string.IsNullOrEmpty(notLike))
                where += $" AND (Message NOT LIKE '%{notLike}%' " +
                    $"OR Exception NOT LIKE '%{notLike}%' " +
                    $"OR CustomException NOT LIKE '%{notLike}%') ";

            return where;
        }
    }
}
