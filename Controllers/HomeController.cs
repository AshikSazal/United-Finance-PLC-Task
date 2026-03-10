using System.Diagnostics;
using Loan_Procedure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loan_Procedure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
