using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TasksApp.Models;

namespace TasksApp.Controllers
{
    [AjaxOnly]
    public class TasksController : Controller
    {
        // GET /Tasks
        [HttpGet]
        public ActionResult Index(int? id)
        {
            using (var dataContext = new DataContext())
            {
                var data = dataContext.Tasks.AsNoTracking();
                var model = id != null ? (object)data.FirstOrDefault(x => x.Id == id && x.User.UserName == User.Identity.Name) : data.Where(x => x.User.UserName == User.Identity.Name).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        // POST /Tasks
        [HttpPost]
        public ActionResult Index(TaskModel model)
        {
            using (var dataContext = new DataContext())
            {
                var item = new Task
                {
                    Title = model.Title,
                    Completed = model.Completed,
                    User = dataContext.UserProfiles.AsNoTracking().First(x => x.UserName == User.Identity.Name)
                };
                dataContext.Tasks.Add(item);
                dataContext.SaveChanges();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
        }

        // DELETE /Tasks/1
        [HttpDelete]
        public ActionResult Index(int id)
        {
            using (var dataContext = new DataContext())
            {
                var item = dataContext.Tasks.FirstOrDefault(x => x.Id == id && x.User.UserName == User.Identity.Name);
                if (item != null)
                {
                    dataContext.Tasks.Remove(item);
                    dataContext.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        // PUT /Tasks/1
        [HttpPut]
        public ActionResult Index(int id, TaskModel model)
        {
            using (var dataContext = new DataContext())
            {
                var item = dataContext.Tasks.FirstOrDefault(x => x.Id == model.Id && x.User.UserName == User.Identity.Name);
                if (item == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                item.Title = model.Title;
                item.Completed = model.Completed;
                dataContext.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
    }
}