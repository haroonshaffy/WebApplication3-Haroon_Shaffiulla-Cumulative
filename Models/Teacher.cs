using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3_Haroon_Shaffiulla_Cumulative.Models
{
    public class Teacher
    {
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public string TeacherHireDate;
        public double TeacherSalary;
        public List<String> TeacherCourse = new List<string>();
        public string inputError = "i";

        public Teacher() { }
    }

}