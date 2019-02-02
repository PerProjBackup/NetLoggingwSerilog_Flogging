using Flogging.Web;
using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using TodoDataAccess;

namespace TodoForms
{
  public partial class ToDos : CustomPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        GetToDosAndBindGrid();
        Helpers.LogWebUsage("ToDos", "WebForms", "View ToDos");
      }
    }

    private void GetToDosAndBindGrid()
    {
      var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

      using (var db = new ToDoDbContext(connStr))
      {
        var todos = db.ToDoItems.ToList();
        gvToDos.DataSource = todos;
        gvToDos.DataBind();
      }
    }

    protected void EditButton_Command(object sender, EventArgs e)
    {
      var todoItemId = (sender as LinkButton).CommandArgument;
      Response.Redirect($"/EditToDo.aspx?Id={todoItemId}");
    }
  }
}