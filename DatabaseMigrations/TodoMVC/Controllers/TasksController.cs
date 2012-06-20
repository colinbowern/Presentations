using System.Net;
using System.Web.Mvc;
using TodoMVC.Models;
using TodoMVC.Persistence;

namespace TodoMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly IRepository repository = new Repository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            return Json(repository.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string name)
        {
            // TODO: Validate input before using it
            var task = new Task {Name = name};
            repository.Add(task);
            return Json(task, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            // TODO: Handle item not found?
            var item = repository.Get(id);
            repository.Delete(item);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult ToggleComplete(int id)
        {
            // TODO: Return actual value in case toggled in another window
            var item = repository.Get(id);
            //item.IsCompleted = !item.IsCompleted;
            item.Status = item.Status == TaskStatus.NotStarted ? TaskStatus.Completed : TaskStatus.NotStarted;
            repository.Update(item);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
