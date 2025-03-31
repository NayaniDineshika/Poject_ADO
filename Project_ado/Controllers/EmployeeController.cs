using Microsoft.AspNetCore.Mvc;
using Project_ado.Models;

namespace Project_ado.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Employee_DAL _dal;


        public EmployeeController(Employee_DAL dal)
        {
            _dal = dal;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = _dal.GetAll();
            }
            catch (Exception ex) 
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View("~/Views/Employee/Index.cshtml", employees);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Renders Create.cshtml
        }


        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Model data is invalid";
                return View(employee);
            }

            bool result = _dal.Add(employee);
            if (!result)
            {
                TempData["errorMessage"] = "Failed to save employee details";
                return View(employee);
            }

            TempData["successMessage"] = "Employee details saved successfully";
            return RedirectToAction("Index"); // Redirect to prevent re-posting
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var employee = _dal.GetEmployeeeById(id);
            if (employee == null)
            {
                TempData["errorMessage"] = "Employee Not Fpond";
                return View(employee);
            }

            return View(employee);
          
        }

        [HttpPost]
        public IActionResult Update (Employee employee)
        {
            if(!ModelState.IsValid)
            {
                TempData["errorMessage"] = "Invalid data";
                return View(employee);
            }

            bool result = _dal.UpdateEmployee(employee);
            if (!result)
            {
                TempData["errorMessage"] = "Failed to update employee";
                return View(employee);
            }
            TempData["successMessage"] = "Employee updated successfully";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Pass only the ID to the DAL method
            bool result = _dal.DeleteEmployee(id);
            if (!result)
            {
                TempData["errorMessage"] = "Failed to delete employee";
                return RedirectToAction("Index");
            }

            TempData["successMessage"] = "Employee deleted successfully";
            return RedirectToAction("Index");
        }





    }

}
