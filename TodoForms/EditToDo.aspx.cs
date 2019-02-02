using Flogging.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using TodoDataAccess;

namespace TodoForms
{
  public partial class EditToDo : CustomPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var todoIdStr = Request.QueryString["Id"];
      var todoId = Convert.ToInt32(todoIdStr);

      if (!IsPostBack)
      {
        var connStr = ConfigurationManager
              .ConnectionStrings["DefaultConnection"].ConnectionString;
        using (var db = new ToDoDbContext(connStr))
        {
          var todo = db.ToDoItems.Single(a => a.Id == todoId);

          ToDoId.Value = todo.Id.ToString();
          ToDoText.Value = todo.Item;
          Completed.Checked = todo.Completed;

          Session["Hello"] = "World";
          Helpers.LogWebDiagnostic("ToDos", "WebForms",
              "Checking on Edit Load method",
              new Dictionary<string, object> { { "Watch", "This" } });
        }
      }
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
      Response.Redirect("/ToDos.aspx");
    }

    protected void UpdateToDoButton_Click(object sender, EventArgs e)
    {
      var text = ToDoText.Value;
      var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      using (var db = new ToDoDbContext(connStr))
      {
        var todoId = Convert.ToInt32(ToDoId.Value);
        var todo = db.ToDoItems.Single(a => a.Id == todoId);

        todo.Item = ToDoText.Value;
        todo.Completed = Completed.Checked;
        db.SaveChanges();

        Helpers.LogWebUsage("ToDos", "WebForms", "Update ToDo");

        Response.Redirect("/ToDos.aspx");
      }
    }
  }
}