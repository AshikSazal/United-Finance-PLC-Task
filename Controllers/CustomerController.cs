using Loan_Procedure.Models;
using Loan_Procedure.Services;
using Microsoft.AspNetCore.Mvc;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        // GET: /Customer
        public IActionResult Index()
        {
            var customers = _service.GetCustomers();
            return View(customers);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            Response response = _service.CreateCustomer(customer);
            if (response.Status) // success
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(customer);
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var customer = _service.GetCustomer(id);

                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer not found.");
                    return View(new Customer());
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(new Customer());
            }
        }

        // Handle POST from form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            Response response = _service.UpdateCustomer(customer);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return RedirectToAction("Index");
        }

    }
}
