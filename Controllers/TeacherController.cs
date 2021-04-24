using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3_Haroon_Shaffiulla_Cumulative.Models;
using System.Diagnostics;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Controllers
{
    //An intermediate between view and Web api controller.
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : Teacher/List
        /// <summary>
        /// This gives us a list of all the teachers.
        /// </summary>
        /// <param name="SearchKey">First name/Last name or both.</param>
        /// <returns>A view with list of teacher objects</returns>
        public ActionResult List(string SearchKey = null)
        {
            //Connects using the TeacherData controller.
            TeacherDataController controller = new TeacherDataController();

            //Calls a method of TeacherData controller and stores the result in the Teacher object.
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);

            //Returns the list of teachers.
            return View(Teachers);
        }

        // GET : Teacher/Show/{id}
        /// <summary>
        /// This method is called to display all the details present for a teacher.
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>All information about a particular teacher referenced by the primary key</returns>
        public ActionResult Show(int id)
        {
            //Creates an instance of the controller.
            TeacherDataController controller = new TeacherDataController();

            // Invokes the FindTeacher method we defined in the API controller
            Teacher newTeacher = controller.FindTeacher(id);

            //Returns the required information
            return View(newTeacher);
        }

        //GET : Teacher/DeleteConfirm/{id}
        /// <summary>
        /// This method is used to confirm deletion when the delete button is clicked.
        /// </summary>
        /// <param name="id">Primary Key.</param>
        /// <returns>A view with the teacher's name and a confirm delete message.</returns>
        public ActionResult DeleteConfirm(int id)
        {
            //Creates a instance of Controller.
            TeacherDataController controller = new TeacherDataController();

            // Invokes the FindTeacher method we defined in the API controller
            Teacher newTeacher = controller.FindTeacher(id);

            //Returns the required information
            return View(newTeacher);
        }
        /// <summary>
        /// The following method is used to delete a particular teacher entry
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        //Post : /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            //Creates a instance of Controller.
            TeacherDataController controller = new TeacherDataController();

            // Invokes the deleteTeacher method we defined in the API controller
            controller.deleteTeacher(id);

            //Once the delete is performed, redirecting the user to the List Teacher's view
            return RedirectToAction("List");
        }

        //Get :/Teacher/new
        /// <summary>
        /// This method is used to provide the user with a form to add new Teacher information
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            return View();
        }

        //POST :/Teacher/Create
        /// <summary>
        /// This method is called when we submit the necessary information after the form is validated
        /// </summary>
        /// <param name="TeacherFname">Teacher's First Name</param>
        /// <param name="TeacherLname">Teacher's Last Name</param>
        /// <param name="EmployeeNumber">Employee Number</param>
        /// <param name="TeacherHireDate">Hiring date</param>
        /// <param name="TeacherSalary">Teacher's salary</param>
        /// <returns></returns>
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, string TeacherHireDate, Double TeacherSalary)
        {
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(TeacherHireDate);
            Debug.WriteLine(TeacherSalary);
            //Creates a model for teacher object.
            Teacher NewTeacher = new Teacher();

            if (TeacherFname == "" || TeacherLname == "" || EmployeeNumber == "" || TeacherHireDate == "")
            {
                return RedirectToAction("InvalidData");
            }
            else
            {
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.EmployeeNumber = EmployeeNumber;

                // Invokes the AddTeacher method we defined in the API controller
                TeacherDataController controller = new TeacherDataController();
                controller.AddTeacher(NewTeacher);

                return RedirectToAction("List");

            }
        }
        /// <summary>
        /// The following view is returned if the form validation fails.
        /// </summary>
        /// <returns>A view </returns>
        //GET : /Teacher/Invalid
        public ActionResult InvalidData()
        {
            return View();
        }
    }


}