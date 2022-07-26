using Microsoft.AspNetCore.Mvc;

namespace CoffeeAutomation.Controllers
{
    public class ErrorController : Controller
    {
        public  IActionResult Error404()
        {

            return View();
        }
    }
}
