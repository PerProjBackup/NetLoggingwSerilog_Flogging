using Flogging.Web;
using Flogging.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TodoDataAccess;

namespace TodoMvc.Controllers
{
  public class ToDosController : Controller
  {
    private ToDoDbContext _db;

    public ToDosController()
    {
      var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      _db = new ToDoDbContext(connStr);
    }

    [TrackUsage(Constants.ProductName, Constants.LayerName, "View ToDos")]
    public ActionResult Index()
    {
      return View(_db.ToDoItems.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TrackUsage(Constants.ProductName, Constants.LayerName, "Create New ToDo")]
    public ActionResult Create(ToDoItem toDoItem)
    {
      if (ModelState.IsValid)
      {
        _db.ToDoItems.Add(toDoItem);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(toDoItem);
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Helpers.LogWebDiagnostic(Constants.ProductName, Constants.LayerName,
          "Just checking in....",
          new Dictionary<string, object> { { "Very", "Important" } });

      var toDoItem = _db.ToDoItems.Find(id);
      if (toDoItem == null)
        throw new Exception($"No To-Do found with item number [{id}]!");

      return View(toDoItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TrackUsage(Constants.ProductName, Constants.LayerName, "Edit ToDo")]
    public ActionResult Edit([Bind(Include = "ID,Item,Completed")] ToDoItem toDoItem)
    {
      if (ModelState.IsValid)
      {
        _db.Entry(toDoItem).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(toDoItem);
    }

    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var toDoItem = _db.ToDoItems.Find(id);
      if (toDoItem == null)
      {
        return HttpNotFound();
      }
      return View(toDoItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      var toDoItem = _db.ToDoItems.Find(id);
      if (toDoItem != null)
      {
        _db.ToDoItems.Remove(toDoItem);
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}