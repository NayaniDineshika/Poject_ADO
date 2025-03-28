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


    }

}
