using Dapper;
using Flogging.Core;
using Flogging.Data.CustomAdo;
using Flogging.Data.CustomDapper;
using FloggingConsole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloggingConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var fd = GetFlogDetail("starting application", null);
      Flogger.WriteDiagnostic(fd);

      var tracker = new PerfTracker("FloggerConsole_Execution", "", fd.UserName,
        fd.Location, fd.Product, fd.Layer);

      //try {
      //  var ex = new Exception("Something bad has happened!");
      //  ex.Data.Add("input param", "nothing to see here");
      //  throw ex;
      //} catch (Exception ex) {
      //  fd = GetFlogDetail("", ex);
      //  Flogger.WriteError(fd); }

      var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
      using (var db = new SqlConnection(connStr))
      {
        db.Open();
        try { //RAW ADO.NET
          //var rawAdoSP = new SqlCommand("CreateNewCustomer", db)
          //{ CommandType = System.Data.CommandType.StoredProcedure };

          //rawAdoSP.Parameters.Add(new SqlParameter("@Name", "waytoolongforitsowngood"));
          //rawAdoSP.Parameters.Add(new SqlParameter("@TotalPurchases", 12000));
          //rawAdoSP.Parameters.Add(new SqlParameter("@TotalReturns", 100.50M));
          //rawAdoSP.ExecuteNonQuery();
          var sp = new Sproc(db, "CreateNewCustomer");
          sp.SetParams("@Name", "waytoolongforitsowngood");
          sp.SetParams("@TotalPurchases", 12000);
          sp.SetParams("@TotalReturns", 100.50M);
          sp.ExecNonQuery();
        } catch (Exception ex) {
          var efd = GetFlogDetail("", ex);
          Flogger.WriteError(efd); }
        
        try { // Dapper
          //db.Execute("CreateNewCustomer", new {
          //  Name = "dappernametoolongtowork", TotalPurchases = 12000, TotalReturns = 100.50M
          //}, commandType: System.Data.CommandType.StoredProcedure );
            db.DapperProcNonQuery("CreateNewCustomer", new { Name = "dappernametoolongtowork",
                  TotalPurchases = 12000, TotalReturns = 100.50M });
        } catch (Exception ex) {
          var efd = GetFlogDetail("", ex);
          Flogger.WriteError(efd); } }

      var ctx = new CustomerDbContext();
      try {
        var name = new SqlParameter("@Name", "waytoolongforitsowngood");
        var totalPurchases = new SqlParameter("@TotalPurchases", 12000);
        var totalReturns = new SqlParameter("@TotalReturns", 100.50M);
        ctx.Database.ExecuteSqlCommand("EXEC dbo.CreateNewCustomer @Name," +
          "@TotalPurchases, @TotalReturns", name, totalPurchases, totalReturns);
      } catch (Exception ex) {
        var efd = GetFlogDetail("", ex);
        Flogger.WriteError(efd); }
                     
      fd = GetFlogDetail("used flogging console", null);
      Flogger.WriteUseage(fd);

      fd = GetFlogDetail("stopping app", null);
      Flogger.WriteDiagnostic(fd);

      tracker.Stop();
    }

    private static FlogDetail GetFlogDetail(string message, Exception ex)
    {
      return new FlogDetail { 
        Product = "Flogger", Location = "FloggerConsole", // this applicaiton
        Layer = "Job", // unattended executable invoked somehow
        UserName  = Environment.UserName, Hostname = Environment.MachineName,
        Message = message, Exception = ex };

    }
  }
}
