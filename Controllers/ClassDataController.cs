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
    // This controller is used to go through the class data that's present in the database
    public class ClassDataController : ApiController
    {
        // Reference to the School database in order to connect with it
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// The following method is used to list the class details.
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>
        /// <returns>List of Classes</returns>
        
        [HttpGet]
        [Route("api/ClassData/ListClasses")]
        public IEnumerable<ClassDetail> ListClasses()
        {
            //Instance of a connection using MySQL object.
            MySqlConnection Connection = School.AccessDatabase();

            //Establishes connection between web server and the database
            Connection.Open();

            //Create a new command of SQL.
            MySqlCommand cmd = Connection.CreateCommand();


            ////Required SQL query to list class and teacher table information
            cmd.CommandText = "SELECT classid,classcode,classname,teacherfname,teacherlname,startdate,finishdate FROM classes c Join teachers t on c.teacherid= t.teacherid";


            //Storing the result in Resultset
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //List that stores all the class data.
            List<ClassDetail> ClassesDetails = new List<ClassDetail> { };

            while (ResultSet.Read())
            {
                //Storing each individual value in it's own variable
                int ClassId = (int)ResultSet["classid"];
                string TeacherfName = (string)ResultSet["teacherfname"];
                string TeacherlName = (string)ResultSet["teacherlname"];
                string classcode = (string)ResultSet["classcode"];
                string classname = (string)ResultSet["classname"];
                DateTime startDate = (DateTime)ResultSet["startdate"];
                DateTime finishdate = (DateTime)ResultSet["finishdate"];
                string start = startDate.ToLongDateString();
                string end = finishdate.ToLongDateString();


                // Creating a NewClassDetails object to store the information of each class
                ClassDetail NewClassDetails = new ClassDetail();

                //Adding the variables to the NewClassDetails object we created
                NewClassDetails.classId = ClassId;
                NewClassDetails.teacherFName = TeacherfName;
                NewClassDetails.teacherlName = TeacherlName;
                NewClassDetails.className = classname;
                NewClassDetails.classCode = classcode;
                NewClassDetails.startDate = start;
                NewClassDetails.FinishDate = end;

                // Storing all this information together in the ClassedDetails List
                ClassesDetails.Add(NewClassDetails);
            }
            //Closing the connection between the database and server
            Connection.Close();

            //Returning the stored List
            return ClassesDetails;
        }
    }
}