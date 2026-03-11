using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Services;
using Loan_Procedure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            PagedResponse<CustomerResponseDto> customers = _service.GetCustomers(page, pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = customers.TotalRecords;

            ViewBag.PageSizes = new List<SelectListItem>
            {
                new () { Text = "5", Value = "5", Selected = (pageSize == 5) },
                new () { Text = "10", Value = "10", Selected = (pageSize == 10) },
                new () { Text = "15", Value = "15", Selected = (pageSize == 15) },
                new () { Text = "20", Value = "20", Selected = (pageSize == 20) }
            };
            return View(customers.Items);
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
