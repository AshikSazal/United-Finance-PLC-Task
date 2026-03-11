using Microsoft.AspNetCore.Mvc;

namespace Loan_Procedure.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }
    }
}
