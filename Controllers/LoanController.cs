using Loan_Procedure.Models;
using Loan_Procedure.Services;
using Microsoft.AspNetCore.Mvc;
using Loan_Procedure.Utils;
using Loan_Procedure.DTOs.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Loan_Procedure.Utils.Constants;

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

        public IActionResult Index(int? status, int? customerId, int page = 1, int pageSize = 5)
        {
            List<CustomerResponseDto> customers = _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name", customerId);
            ViewBag.SelectedStatus = status;
            var statusList = LoanStatusList.Status;
            ViewBag.Statuses = new SelectList(statusList, "Value", "Text", status);

            var pagedLoans = _service.GetLoans(status, customerId, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = pagedLoans.TotalRecords;


            ViewBag.PageSizes = new List<SelectListItem>
            {
                new () { Text = "5", Value = "5", Selected = (pageSize == 5) },
                new () { Text = "10", Value = "10", Selected = (pageSize == 10) },
                new () { Text = "15", Value = "15", Selected = (pageSize == 15) },
                new () { Text = "20", Value = "20", Selected = (pageSize == 20) }
            };

            return View(pagedLoans.Items);
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
            if (response.Status)
            {
                TempData["Message"] = response.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(loan);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                List<CustomerResponseDto> customers = _customerService.GetCustomers();
                ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");

                var loan = _service.GetLoan(id);

                if (loan == null)
                {
                    ModelState.AddModelError(string.Empty, "Loan not found.");
                    return View(new Loan());
                }

                return View(loan);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error loading loan: " + ex.Message);
                return View(new Loan());
            }
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
            }

            return RedirectToAction("Index");
        }
        public IActionResult Approve(int id)
        {
            Response response = _service.UpdateStatus(id, 2);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
            }

            return RedirectToAction("Index");
        }
        public IActionResult Reject(int id)
        {
            Response response = _service.UpdateStatus(id, 3);
            if (response.Status)
            {
                TempData["Message"] = response.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
