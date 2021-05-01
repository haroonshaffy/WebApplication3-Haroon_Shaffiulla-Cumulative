using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using WebApplication3_Haroon_Shaffiulla_Cumulative.Models;
using System.Diagnostics;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Controllers
{
    // This controller is used to go through the Teacher data that's present in the database
    public class TeacherDataController : ApiController
    {
        // Reference to the School database in order to connect with it
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// The following method is used to list the names of the teachers. 
        /// If a searchkey is provided then only that teacher's information will be listed
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <example>GET api/TeacherData/ListTeachers/Alexander</example>
        /// <param name="searchKey">The searchKey is either the first name/last name or both</param>
        /// <returns>List of Teachers</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{searchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string searchKey = null)
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Create a new command of SQL.
            MySqlCommand cmd = Connection.CreateCommand();


            //Required SQL query to find teachers with the particular searchKey
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            // Telling the system to search for teacher names that contain the "SearchKey" in any part of it's string 
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");

            //Storing the result in Resultset
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //List that stores all the teacher data.
            List<Teacher> TeachersNames = new List<Teacher> { };

            while (ResultSet.Read())
            {
                //Storing each individual value in it's own variable
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherfName = (string)ResultSet["teacherfname"];
                string TeacherlName = (string)ResultSet["teacherlname"];
                DateTime Hiredate = (DateTime)ResultSet["hiredate"];
                string TeacherHireDate = Hiredate.ToLongDateString();
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);

                // Creating a NewTeacher object of type Teacher to store the information of one teacher at a time.
                Teacher NewTeacher = new Teacher();

                //Adding the variables to the NewTeacher object we created

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherfName;
                NewTeacher.TeacherLname = TeacherlName;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;

                // Storing all this information together in the TeachersNames List
                TeachersNames.Add(NewTeacher);
            }
            //Closing the connection between the database and server
            Connection.Close();

            //Returning the stored List
            return TeachersNames;
        }


        /// We use this method to show which teacher is assigned which course
        /// We're using the class table from the school DB
        /// <summary>
        /// This method lists teacher information based on the primary key we provide.
        /// </summary>
        /// <param name="id">Primary key from the teacher table.</param>
        /// <returns>A teacher Object with all information</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            // Creating a Teacher object to store the result.
            Teacher NewTeacher = new Teacher();

            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Creating a new SQL command
            MySqlCommand cmd = Connection.CreateCommand();

            // SQL Query to find teacher by Id.
            cmd.CommandText = "SELECT * FROM teachers where teacherid = " + id;

            //Storing the result in Resultset
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Storing each individual value in it's own variable
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherfName = (string)ResultSet["teacherfname"];
                string TeacherlName = (string)ResultSet["teacherlname"];
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);
                string date = TeacherHireDate.ToLongDateString();


                // Adding the variables to the NewTeacher object we created
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherfName;
                NewTeacher.TeacherLname = TeacherlName;
                NewTeacher.TeacherHireDate = date;
                NewTeacher.TeacherSalary = TeacherSalary;
            }

            //Closing the connection between the database and server
            ResultSet.Close();

            // Pulling class information associated with the Teacher ID
            cmd.CommandText = "SELECT classname from classes where teacherid =" + id;

            // Storing the result in Resultset
            ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // To store the course Name information
                string TeacherCourse = (string)ResultSet["classname"];

                //Adding this to the object
                NewTeacher.TeacherCourse.Add(TeacherCourse);
            }
          
            ResultSet.Close();

            //Closing the connection between the server and DB
            Connection.Close();

            //Returning the stored List
            return NewTeacher;
        }

        /// <summary>
        /// The following method is used to delete a teacher entry
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <return></return>
        [HttpPost]
        public void deleteTeacher(int id)
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Creating a new SQL command
            MySqlCommand cmd = Connection.CreateCommand();

            //SQL QUERY to delete by Id
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Closing the connection between the server and DB
            Connection.Close();
        }
        /// <summary>
        /// This method is used to add a new teacher to the table.
        /// </summary>
        /// <param name="newTeacher">Necessary Teacher information.</param>
        /// <returns></returns>
        [HttpPost]
        public void AddTeacher([FromBody] Teacher newTeacher)
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Creating a new SQL command
            MySqlCommand cmd = Connection.CreateCommand();

            //SQL QUERY to Insert values into the Teacher table
            cmd.CommandText = "INSERT INTO teachers " +
                "(teacherfname, teacherlname, employeenumber, hiredate, salary) " +
                "VALUES (@fname, @lname, @employeenumber,@date,@salary);";
            cmd.Parameters.AddWithValue("@fname", newTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@lname", newTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", newTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(newTeacher.TeacherHireDate));
            cmd.Parameters.AddWithValue("@salary", newTeacher.TeacherSalary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Closing the connection between the server and DB
            Connection.Close();
        }

        // <summary>
        /// This method is used when we want to update information about a teacher.
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="teacherInfo">Teacher's Information</param>
    
        [HttpPost]
        public void UpdateTeacher(int id, [FromBody] Teacher teacherInfo)
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Creating a new SQL command
            MySqlCommand cmd = Connection.CreateCommand();

            //SQL QUERY to Update 
            cmd.CommandText = "UPDATE teachers set " +
                "teacherfname = @fname , teacherlname = @lname, salary = @salary " +
                "where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@fname", teacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@lname", teacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@salary", teacherInfo.TeacherSalary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Closing the connection between the server and DB
            Connection.Close();
        }
    }
}