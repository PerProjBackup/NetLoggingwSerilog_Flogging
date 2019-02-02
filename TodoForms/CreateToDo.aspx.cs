using Flogging.Web;
using System;
using System.Configuration;
using System.Web.UI;
using TodoDataAccess;

namespace TodoForms
{
  public partial class CreateToDo : CustomPage
  {
    protected void Page_Load(object sender, EventArgs e)
    { }

    protected void Unnamed_Click(object sender, EventArgs e)
    { Response.Redirect("/ToDos.aspx"); }

    protected void SaveNewToDoButton_Click(object sender, EventArgs e)
    {
      var text = ToDoText.Value;
      var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      using (var db = new ToDoDbContext(connStr))
      {
        var todo = new ToDoItem();
        todo.Item = text;
        db.ToDoItems.Add(todo);
        db.SaveChanges();

        Helpers.LogWebUsage("ToDos", "WebForms", "Create New ToDo");

        Response.Redirect("/ToDos.aspx");
      }
    }
  }
}