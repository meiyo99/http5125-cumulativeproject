using Microsoft.AspNetCore.Mvc;
using cumulative1.Models;

namespace cumulative1.Controllers
{
    public class TeacherPageController : Controller
    {

        private readonly TeacherAPIController _api;
        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: /TeacherPage/{id} -> Returns a teacher by their ID.
        [HttpGet]
        public IActionResult Show(int id)
        {
            //Teacher Object
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // Direct to Views/TeacherPage/Show.cshtml
            return View(SelectedTeacher);
        }

        // GET: /TeacherPage/List?SearchKey={SearchKey} -> Shows a list of teachers filtered by the {SearchKey}.
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            List<Teacher> Teachers = _api.ListTeachers(SearchKey);

            // Direct to Views/TeacherPage/List.cshtml
            return View(Teachers);
        }

        // GET: /TeacherPage/New -> Returns a webpage that asks user for new teacher information.
        [HttpGet]
        public IActionResult New()
        {
            // Direct to Views/TeacherPage/New.cshtml
            return View();
        }

        // POST: /TeacherPage/Create -> Adds a new teacher to the database.
        // HEADER: Content-Type: application/x-www-form-urlencoded
        //FORM DATA: ?FirstName={FirstName}&LastName={LastName}&EmployeeNum={EmployeeNum}&HireDate={HireDate}&Salary={Salary}
        [HttpPost]
        public IActionResult Create(string FirstName, string LastName, string EmployeeNum, DateTime HireDate, decimal Salary)
        {
            Teacher NewTeacher = new Teacher();
            NewTeacher.FirstName = FirstName;
            NewTeacher.LastName = LastName;
            NewTeacher.EmployeeNum = EmployeeNum;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            // Add teacher details to our database
            _api.AddTeacher(NewTeacher);

            // Take us back to /TeacherPage/List.cshtml
            return RedirectToAction("List");
        }

        // GET: /TeacherPage/DeleteConfirm/{TeacherID} -> A webpage that confirms with user if they want to delete this teacher.
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            //Information about selected teacher
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // Direct to Views/TeacherPage/DeleteConfirm.cshtml
            return View(SelectedTeacher);
        }

        // POST: /TeacherPage/Delete/{TeacherID} -> Delete teacher at specified ID.
        [HttpPost]
        public IActionResult Delete(int id)
        {

            _api.DeleteTeacher(id);

            // Take us back to /TeacherPage/List.cshtml
            return RedirectToAction("List");
        }
    }
}
