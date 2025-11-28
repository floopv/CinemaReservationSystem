using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantData.Super_Admin_Role)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if(await _userManager.IsInRoleAsync(user,ConstantData.Super_Admin_Role))
            {
                TempData["Error"] = "Cannot lock/unlock a Super Admin user.";
                return RedirectToAction(nameof(Index));
            }
            if(user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            {
                user.LockoutEnd = null;
                TempData["Success"] = "User unlocked successfully.";
            }
            else
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(10);
                TempData["Success"] = "User locked successfully.";
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
