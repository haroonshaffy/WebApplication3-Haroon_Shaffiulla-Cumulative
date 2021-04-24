using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using WebApplication3_Haroon_Shaffiulla_Cumulative.Models;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Controllers
{
    public class StudentDataController : ApiController
    {
        // Reference to the School database in order to connect with it
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// The following method is used to list the names of the students. 
        /// If a searchkey is provided then only that student's information will be listed
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <example>GET api/StudentData/ListStudents/linda</example>
        /// <param name="searchKey">The searchKey is either the first name/last name or both</param>
        /// <returns>List of Students.</returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{searchKey?}")]
        public IEnumerable<Student> ListStudents(string searchKey)
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Create a new command of SQL.
            MySqlCommand cmd = Connection.CreateCommand();


            //Required SQL query to find students with the particular searchKey
            cmd.CommandText = "Select * from students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname, ' ', studentlname)) like lower(@key)";

            // Telling the system to search for student names that contain the "SearchKey" in any part of it's string
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");

            //Storing the result in Resultset
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //List that stores all the student data.
            List<Student> StudentNames = new List<Student> { };

            while (ResultSet.Read())
            {
                //Storing each individual value in it's own variable
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentfName = (string)ResultSet["studentfname"];
                string studentlName = (string)ResultSet["studentlname"];
                string studentNumber = (string)ResultSet["studentnumber"];
                DateTime studentEnrolDate = (DateTime)ResultSet["enroldate"];
                string date = studentEnrolDate.ToLongDateString();

                // Creating a NewStudent object of type Teacher to store the information of one teacher at a time.
                Student NewStudent = new Student();

                //Adding the variables to the NewStudent object we created

                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentfName;
                NewStudent.StudentLname = studentlName;
                NewStudent.StudentNumber = studentNumber;
                NewStudent.StudentEnrolDate = date;

                // Storing all this information together in the StudentNames List
                StudentNames.Add(NewStudent);
            }
            //Closing the connection between the database and server
            Connection.Close();

            //Returning the stored List
            return StudentNames;
        }

        ///<summary>
        /// This method lists student information based on the primary key we provide.
        /// </summary>
        /// <param name="id">Primary key from the student table.</param>
        /// <returns>A student Object with all necessary information</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            // Creating a student object to store the result.
            Student NewStudent = new Student();

            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Creating a new SQL command
            MySqlCommand cmd = Connection.CreateCommand();


            // SQL Query to find student by Id.
            cmd.CommandText = "Select * from students where studentid =" + id;

            //Storing the result in Resultset
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Storing each individual value in it's own variable
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentfName = (string)ResultSet["studentfname"];
                string studentlName = (string)ResultSet["studentlname"];
                string studentNumber = (string)ResultSet["studentnumber"];
                DateTime studentEnrolDate = (DateTime)ResultSet["enroldate"];
                string date = studentEnrolDate.ToLongDateString();

                // Adding the variables to the NewTeacher object we created

                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentfName;
                NewStudent.StudentLname = studentlName;
                NewStudent.StudentNumber = studentNumber;
                NewStudent.StudentEnrolDate = date;
            }
            //Closing the connection between the server and DB
            Connection.Close();

            //Returning the stored List
            return NewStudent;
        }
    }
}