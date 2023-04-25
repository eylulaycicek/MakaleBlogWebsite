using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakaleWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Test test = new Test();
            return View();
        }
    }
}