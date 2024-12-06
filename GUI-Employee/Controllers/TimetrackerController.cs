using BLL;
using DTO.Models;
using GUI_Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace GUI_Employee.Controllers
{
    public class TimetrackerController : Controller
    {

        [HttpGet]
        public IActionResult Index(EmployeeAndDepartmentModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Stop(int timetrackerId, int employeeId)
        {
            bool success = TimetrackerLogic.StopTimetracker(timetrackerId, employeeId);
            if (!success)
            {
                return RedirectToAction("Index", "Home");
            }

            Employee employee = EmployeeLogic.GetEmployee(employeeId);
            Timetracker timetracker = employee.Timetrackers.Find(t => t.Id == timetrackerId);
            Department department = DepartmentLogic.GetDepartment(timetracker.DepartmentId);
            var model = new EmployeeAndDepartmentModel(employee, department);

            return View("~/Views/Employee/Timetracker/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Start(int employeeId, int departmentId, int? selectedCaseId)
        {

            if (selectedCaseId == null)
            {
                TimetrackerLogic.CreateTimetracker(employeeId, departmentId, DateTime.Now, null);
            }
            else
            {
                TimetrackerLogic.CreateTimetracker(employeeId, departmentId, (int)selectedCaseId, DateTime.Now, null);
            }

            var model = new EmployeeAndDepartmentModel(EmployeeLogic.GetEmployee(employeeId), DepartmentLogic.GetDepartment(departmentId));

            return View("~/Views/Employee/Timetracker/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Create(int employeeId, int departmentId, int? selectedCaseId, DateTime startDateTime, DateTime? endDateTime)
        {
            if (endDateTime != null && startDateTime > endDateTime)
            {
                return View("~/Views/Employee/Timetracker/Index.cshtml", new EmployeeAndDepartmentModel(EmployeeLogic.GetEmployee(employeeId), DepartmentLogic.GetDepartment(departmentId)));
            }

            if (selectedCaseId == null)
            {
                TimetrackerLogic.CreateTimetracker(employeeId, departmentId, startDateTime, endDateTime);
            }
            else
            {
                TimetrackerLogic.CreateTimetracker(employeeId, departmentId, (int)selectedCaseId, startDateTime, endDateTime);
            }

            var model = new EmployeeAndDepartmentModel(EmployeeLogic.GetEmployee(employeeId), DepartmentLogic.GetDepartment(departmentId));

            return View("~/Views/Employee/Timetracker/Index.cshtml", model);
        }
    }
}
