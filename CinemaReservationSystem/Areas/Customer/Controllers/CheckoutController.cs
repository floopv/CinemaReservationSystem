using Microsoft.AspNetCore.Mvc;

namespace CinemaReservationSystem.Areas.Customer.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult success()
        {
            return View();
        }
        public IActionResult cancel()
        {
            return View();
        }
    }
}
