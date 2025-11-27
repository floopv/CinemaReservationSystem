using CinemaReservationSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstantData.Admin_Role} , {ConstantData.Super_Admin_Role}")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
