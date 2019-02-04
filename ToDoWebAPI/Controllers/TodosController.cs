using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers
{
  public class TodosController : ApiController
  {
    private ToDoDbContext _db;

    public TodosController()
    {
      var connStr = ConfigurationManager
        .ConnectionStrings["DefaultConnection"]
        .ConnectionString;
      _db = new ToDoDbContext(connStr);
    }

    public IQueryable<ToDoItem> Get()
    {
      //var todos = _db.ToDoItems.Include("garbage").AsQueryable();
      //return todos;
      return _db.ToDoItems.AsQueryable();
    }

    public ToDoItem Get(int id)
    {
      return _db.ToDoItems.Find(id);
    }

    public int Post([FromBody]ToDoItem item)
    {
      _db.ToDoItems.Add(item);
      _db.SaveChanges();
      return item.Id;
    }

    public void Put(int id, [FromBody]ToDoItem item)
    {
      var itemToUpdate = _db.ToDoItems.Find(id);
      itemToUpdate.Item = item.Item;
      itemToUpdate.Completed = item.Completed;
      _db.SaveChanges();
    }

    public void Delete(int id)
    {
      var itemToRemove = _db.ToDoItems.Find(id);
      _db.ToDoItems.Remove(itemToRemove);
    }
  }
}
