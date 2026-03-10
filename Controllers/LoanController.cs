using Loan_Procedure.Models;
using Loan_Procedure.Services;
using Microsoft.AspNetCore.Mvc;
using Loan_Procedure.Utils;
using Loan_Procedure.DTOs.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loan_Procedure.Controllers
{
    public class LoanController : Controller
    {
        private readonly LoanService _service;
        private readonly CustomerService _customerService;

        public LoanController(LoanService service, CustomerService customerService)
        {
            _service = service;
            _customerService = customerService;
        }

        public IActionResult Index(int? status, int? customerId)
        {
            List<CustomerResponseDto> customers = _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name", customerId);
            var loans = _service.GetLoans(status, customerId);
            ViewBag.SelectedStatus = status;
            var statusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- All Status --" },
                new SelectListItem { Value = "0", Text = "Draft" },
                new SelectListItem { Value = "1", Text = "Submitted" },
                new SelectListItem { Value = "2", Text = "Approved" },
                new SelectListItem { Value = "3", Text = "Rejected" }
            };

            ViewBag.Statuses = new SelectList(statusList, "Value", "Text", status);

            return View(loans);
        }

        public IActionResult Create()
        {
            List<CustomerResponseDto> customers = _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Loan loan)
        {
            List<CustomerResponseDto> customers = _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");

            if (!ModelState.IsValid)
                return View(loan);

            Response response = _service.CreateLoan(loan);
            if (response.Status) // success
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            // If there is an error, show it on the same page
            ModelState.AddModelError(string.Empty, response.Message);
            return View(loan);
        }

        public IActionResult Edit(int id)
        {
            List<CustomerResponseDto> customers = _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
            var loan = _service.GetLoan(id);

            if (loan == null)
                return View(null);

            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Loan loan)
        {
            if (!ModelState.IsValid)
                return View(loan);

            Response response = _service.UpdateLoan(loan);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(loan);
        }

        public IActionResult Submit(int id)
        {
            Response response = _service.UpdateStatus(id, 1);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        public IActionResult Approve(int id)
        {
            Response response = _service.UpdateStatus(id, 2);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        public IActionResult Reject(int id)
        {
            Response response = _service.UpdateStatus(id, 3);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
