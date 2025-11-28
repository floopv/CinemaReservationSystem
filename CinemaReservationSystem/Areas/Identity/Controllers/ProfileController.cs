using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var userVM = user.Adapt<ApplicationUserVM>();
            return View(userVM);
        }
        public async Task<IActionResult> UpdateProfile(ApplicationUserVM applicationUserVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var names = applicationUserVM.FullName?.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            if(names != null && names.Length >= 2)
            {
                user.FirstName = names[0];
                user.LastName = names[1];
            }
            else
            {
                user.FirstName = applicationUserVM.FullName;
                user.LastName = string.Empty;
            }
            user.PhoneNumber = applicationUserVM.PhoneNumber;
            user.Address = applicationUserVM.Address;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index) , applicationUserVM);
            }
        }
        public async Task<IActionResult> UpdatePassword(ApplicationUserVM applicationUserVM)
        {
            if(string.IsNullOrEmpty(applicationUserVM.CurrentPassword) || string.IsNullOrEmpty(applicationUserVM.NewPassword))
            {
                TempData["Error"] = "Please provide both current and new passwords.";
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var result = await _userManager.ChangePasswordAsync(user, applicationUserVM.CurrentPassword, applicationUserVM.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password updated successfully.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
