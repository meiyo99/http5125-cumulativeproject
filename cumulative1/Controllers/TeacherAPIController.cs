using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cumulative1.Models;
using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace cumulative1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        /// <summary>
        /// This method returns all the teachers from the database.
        /// </summary>
        /// <returns>
        /// Returns a list of Teachers.
        /// </returns>
        /// <example>
        /// GET: api/TeacherAPI/ListTeachers -> 
        /// [{"teacherID":1,"firstName":"Alexander","lastName":"Bennett","employeeNum":"T378","hireDate":"2016-08-05 00:00:00","salary":55.30}
        /// {"teacherID":2,"firstName":"Caitlin","lastName":"Cummings","employeeNum":"T381","hireDate":"2014-06-10 00:00:00","salary":62.77}
        /// {"teacherID":3,"firstName":"Linda","lastName":"Chan","employeeNum":"T385","hireDate":"2014-06-22T00:00:00","salary":74.20}]
        /// </example>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            List<Teacher> Teachers = new List<Teacher>();

            // connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // accessing the database
            using (MySqlConnection Connection = school.AccessDatabase())
            {

                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                string query = "select * from teachers";
                Command.CommandText = query;

                // run the sql command
                MySqlDataReader ResultSet = Command.ExecuteReader();

                // for each entry in our database
                while (ResultSet.Read())
                {
                    int TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                    string FirstName = ResultSet["teacherfname"].ToString();
                    string LastName = ResultSet["teacherlname"].ToString();
                    string EmployeeNum = ResultSet["employeenumber"].ToString();
                    DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                    decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                    Teacher NewTeacher = new Teacher();
                    NewTeacher.TeacherID = TeacherID;
                    NewTeacher.FirstName = FirstName;
                    NewTeacher.LastName = LastName;
                    NewTeacher.EmployeeNum = EmployeeNum;
                    NewTeacher.HireDate = HireDate;
                    NewTeacher.Salary = Salary;

                    // add teacher name in the list
                    Teachers.Add(NewTeacher);
                }
            }

            return Teachers;
        }

        /// <summary>
        /// This method returns all information for a Teacher by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A teacher object.
        /// </returns>
        /// <example>
        /// GET: api/TeacherAPI/FindTeacher/4 -> 
        /// {"teacherID":4,"firstName":"Lauren","lastName":"Smith","employeeNum":"T385","hireDate":"2014-06-22T00:00:00","salary":74.20}
        /// </example>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher SelectedTeacher = new Teacher();

            //connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // accessing the database
            using (MySqlConnection Connection = school.AccessDatabase())
            {

                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                string query = "select * from teachers where teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);
                Command.CommandText = query;

                // MySqlDataReader
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {
                        SelectedTeacher.TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                        SelectedTeacher.FirstName = ResultSet["teacherfname"].ToString();
                        SelectedTeacher.LastName = ResultSet["teacherlname"].ToString();
                        SelectedTeacher.EmployeeNum = ResultSet["employeenumber"].ToString();
                        SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        SelectedTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                    }

                }

            }

            return SelectedTeacher;
        }

        /// <summary>
        /// Returns a list of teachers that match the input search keyword.
        /// </summary>
        /// <param name="SearchKey">The text to search teachers against</param>
        /// <returns>A list of teacher objects.</returns>
        /// <example>
        /// GET: api/ListTeachers/Linda -> 
        /// {"teacherID":3,"firstName":"Linda","lastName":"Chan","employeeNum":"T385","hireDate":"2014-06-22T00:00:00","salary":74.20}
        /// </example>
        [HttpGet]
        [Route(template: "ListTeachers/{SearchKey}")]
        public List<Teacher> ListTeachers(string SearchKey)
        {
            List<Teacher> Teachers = new List<Teacher>();

            // connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // accessing the database
            using (MySqlConnection Connection = school.AccessDatabase())
            {

                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                string query = "select * from teachers where lower(teacherfname) LIKE @key OR lower(teacherlname) LIKE @key OR lower(CONCAT(teacherfname,' ',teacherlname)) LIKE @key";
                Command.CommandText = query;
                Command.Parameters.AddWithValue("@key", "%"+SearchKey+"%");
                Command.Prepare();

                // run the sql command
                MySqlDataReader ResultSet = Command.ExecuteReader();

                // for each entry in our database
                while (ResultSet.Read())
                {
                    int TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                    string FirstName = ResultSet["teacherfname"].ToString();
                    string LastName = ResultSet["teacherlname"].ToString();
                    DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                    decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                    Teacher NewTeacher = new Teacher();
                    NewTeacher.TeacherID = TeacherID;
                    NewTeacher.FirstName = FirstName;
                    NewTeacher.LastName = LastName;
                    NewTeacher.HireDate = HireDate;
                    NewTeacher.Salary = Salary;

                    //add teacher name in the list
                    Teachers.Add(NewTeacher);
                }
            }

            return Teachers;
        }

        /// <summary>
        /// This method will receive teacher information add a teacher in our database.
        /// </summary>
        /// <returns>
        /// "Teacher Details added at ID:{TeacherId}"
        /// </returns>
        /// <example>
        /// POST: api/TeacherAPI/AddTeacher
        /// HEADER: Content-Type: application/json
        /// FORM DATA: 
        /// {"FirstName":"Mayuresh","LastName":"Naidu","EmployeeNum":"M123","HireDate":"2025-04-12","Salary":"45.67"}
        /// </example>
        [HttpPost]
        [Route(template: "/api/TeacherAPI/AddTeacher")]
        public string AddTeacher([FromBody] Teacher NewTeacher)
        {
            // server validation
            // validating the FirstName and the LastName fields
            if (string.IsNullOrEmpty(NewTeacher.FirstName) || string.IsNullOrEmpty(NewTeacher.LastName))
            {
                return "Teacher Name Field is Empty.";
            }

            // validating the HireDate field
            if (NewTeacher.HireDate > DateTime.Now)
            {
                return "Hire Date cannot be after Current Date.";
            }

            int TeacherID = 0;

            // connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // accessing the database
            using (MySqlConnection Connection = school.AccessDatabase())
            {

                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@firstname, @lastname, @employeenum, @hiredate, @salary)";
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@firstname", NewTeacher.FirstName);
                Command.Parameters.AddWithValue("@lastname", NewTeacher.LastName);
                Command.Parameters.AddWithValue("@employeenum", NewTeacher.EmployeeNum);
                Command.Parameters.AddWithValue("@hiredate", NewTeacher.HireDate);
                Command.Parameters.AddWithValue("@salary", NewTeacher.Salary);

                // run this query against the database
                Command.ExecuteNonQuery();

                TeacherID = Convert.ToInt32(Command.LastInsertedId);

            }
            return "Teacher Details added at ID:" + TeacherID;
        }

        /// <summary>
        /// This method will delete a teacher from the database.
        /// </summary>
        /// <param name="id">The primary key</param>
        /// <returns>
        /// "Teacher Field Deleted Succesfully!"
        /// </returns>
        /// <example>
        /// DELETE: api/DeleteTeacher/6 -> "Teacher Field Deleted Succesfully!"
        /// </example>
        [HttpDelete]
        [Route(template: "/api/TeacherAPI/DeleteTeacher/{id}")]
        public string DeleteTeacher(int id)
        {
            // connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // accessing the database
            using (MySqlConnection Connection = school.AccessDatabase())
            {
                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                string query = "delete from teachers where teacherid = @id";
                Command.CommandText = query;
                Command.Parameters.AddWithValue("@id", id);
                Command.ExecuteNonQuery();

                int rowsAffected = Command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return "Invalid ID";
                }
            }
            return "Teacher Field Deleted Succesfully!"; ;
        }

        /// <summary>
        /// We want to receive teacher information and update teacher details in the database.
        /// </summary>
        /// <returns>
        /// The updated teacher object.
        /// </returns>
        /// <example>
        /// PUT: /api/TeacherAPI/UpdateTeacher/{id}
        /// Header: Content-Type: application/json
        /// FORM DATA:
        /// </example>
        [HttpPut]
        [Route(template:"api/TeacherAPI/UpdateTeacher/{id}")]
        public string UpdateTeacher(int id, [FromBody] Teacher UpdatedTeacher)
        {

            // connect to school database
            SchoolDBContext school = new SchoolDBContext();

            // set up an sql command
            string query = "update teachers set teacherfname = @firstname, teacherlname = @lastname, employeenumber = @employeenum, hiredate = @hiredate, salary = @salary where teacherid = @id";

            using (MySqlConnection Connection = school.AccessDatabase())
            {

                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // set up an sql command
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@id", id);
                Command.Parameters.AddWithValue("@firstname", UpdatedTeacher.FirstName);
                Command.Parameters.AddWithValue("@lastname", UpdatedTeacher.LastName);
                Command.Parameters.AddWithValue("@employeenum", UpdatedTeacher.EmployeeNum);
                Command.Parameters.AddWithValue("@hiredate", UpdatedTeacher.HireDate);
                Command.Parameters.AddWithValue("@salary", UpdatedTeacher.Salary);

                // run this query against the database
                Command.ExecuteNonQuery();

            }

            return "Details Updated!";
        }

    }
}
