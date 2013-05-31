using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TasksApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return User.Identity.IsAuthenticated ? View() : View("Index.Unauthenticated");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
