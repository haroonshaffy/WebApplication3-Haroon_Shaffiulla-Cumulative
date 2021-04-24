using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3_Haroon_Shaffiulla_Cumulative.Models;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Controllers
{
    //An intermediate between view and Web api controller.
    public class ClassesController : Controller
    {
        // GET: Classes
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This gives us a list of all the classes.
        /// </summary>
        /// <returns>A view with list of ClassDetail's objects</returns>
        public ActionResult List()
        {
            //Connects using the ClassData controller.
            ClassDataController controller = new ClassDataController();
            //Calls a method of ClassData controller and stores the result in the ClassDetail object.
            IEnumerable<ClassDetail> Details = controller.ListClasses();

            //Returns the list of classes.
            return View(Details);
        }
    }
}