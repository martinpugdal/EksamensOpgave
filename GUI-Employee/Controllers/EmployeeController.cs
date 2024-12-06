using BLL;
using DTO.Models;
using GUI_Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace GUI_Employee.Controllers
{
    public class EmployeeController : Controller
    {

        public IActionResult Index(Employee? employee)
        {
            if (employee == null || employee.Id == 0)
                return RedirectToAction("Index", "Home");
            else
                return View(employee);
        }

        [HttpPost]
        public IActionResult Index(int employeeId)
        {
            var employee = EmployeeLogic.GetEmployee(employeeId);
            var timetrackers = TimetrackerLogic.GetWeeklyTimetrackersForEmployee(employeeId).
                Where(t => t.DateTimeEnd != null).
                ToList();

            employee.TotalHours = timetrackers.
                Sum(t => (t.DateTimeEnd - t.DateTimeStart).Value.TotalHours);

            return View("index", employee);
        }

        [HttpPost]
        public IActionResult Timetracker(IFormCollection formCollection)
        {
            var departmentId = Convert.ToInt32(formCollection["departmentId"]);
            var employeeId = Convert.ToInt32(formCollection["employeeId"]);

            Department department = DepartmentLogic.GetDepartment(departmentId);
            Employee employee = EmployeeLogic.GetEmployee(employeeId);

            if (department == null || employee == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var timetrackers = TimetrackerLogic.GetWeeklyTimetrackersForEmployee(employeeId).
                Where(t => t.DateTimeEnd != null).
                ToList();

            employee.TotalHours = timetrackers.
                Sum(t => (t.DateTimeEnd - t.DateTimeStart).Value.TotalHours);

            var model = new EmployeeAndDepartmentModel(employee, department);

            return View("Timetracker/Index", model);
        }
    }
}
