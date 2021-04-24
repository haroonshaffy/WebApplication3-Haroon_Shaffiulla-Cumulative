using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3_Haroon_Shaffiulla_Cumulative.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Controllers
{
    //An intermediate between view and Web api controller.

    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This gives us a list of all the teachers.
        /// </summary>
        /// <param name="SearchKey">First name/Last name or both.</param>
        /// <returns>A view with list of Student objects</returns>
        public ActionResult List(string searchKey = null)
        {
            //Connects using the StudentData controller.
            StudentDataController controller = new StudentDataController();
            //Calls a method of StudentData controller and stores the result in the Student object.
            IEnumerable<Student> Students = controller.ListStudents(searchKey);

            //Returns the list of students.
            return View(Students);
        }


        // GET : Student/Show/{id}
        /// <summary>
        /// This method is called to display all the details present for a student.
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>All information about a particular student referenced by the primary key</returns>
        public ActionResult Show(int id)
        {
            //Creates an instance of the controller.
            StudentDataController controller = new StudentDataController();
            // Invokes the FindStudent method we defined in the API controller
            Student NewStudent = controller.FindStudent(id);
            Debug.WriteLine("The student's details are:");
            Debug.WriteLine(NewStudent);

            //Returns the required information
            return View(NewStudent);
        }
    }
}