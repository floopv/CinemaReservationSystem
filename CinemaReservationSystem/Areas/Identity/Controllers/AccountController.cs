using CinemaReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid registration details.");
                return View(registerVM);
            }
            if (registerVM is not null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    UserName = registerVM.UserName,
                    Email = registerVM.Email
                };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerVM);
                }
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(ConfirmEmail) ,"Account" ,new {area = "Identity", token, userId = user.Id } , Request.Scheme);
                string htmlMessage = $@"
<div style='font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); overflow: hidden;'>
        <div style='background-color: #007bff; color: white; padding: 15px; text-align: center;'>
            <h2 style='margin: 0;'>Cinema Reservation</h2>
        </div>
        <div style='padding: 20px;'>
            <h3 style='color: #333;'>Confirm your email address</h3>
            <p style='color: #555; line-height: 1.6;'>
                Hi <strong>{registerVM.FirstName}</strong>,<br><br>
                Thank you for registering with <strong>Cinema Reservation</strong>.<br>
                Please click the button below to confirm your email address.
            </p>

            <div style='text-align: center; margin: 30px 0;'>
                <a href='{link}'
                   style='background-color: #007bff; color: white; padding: 12px 25px;
                          text-decoration: none; border-radius: 6px; display: inline-block;'>
                    Confirm Email
                </a>
            </div>

            <p style='font-size: 14px; color: #777;'>
                If you did not create this account, please ignore this email.
            </p>
        </div>
        <div style='background-color: #f0f0f0; text-align: center; padding: 10px; font-size: 12px; color: #999;'>
            © 2025 Ecommerce 520. All rights reserved.
        </div>
    </div>
</div>";
                await _emailSender.SendEmailAsync(registerVM.Email,"CinemaReservation Confirmation Mail",htmlMessage);
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ConfirmEmail(string token , string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Email Confirmation Failed";
            }
            else
            {
                TempData["Success"] = "Email Confirmed Successfully";
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            if(resendEmailConfirmationVM == null)
            {
                ModelState.AddModelError("", "Invalid Inputs");
                return View(resendEmailConfirmationVM);
            }
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.EmailOrUserName) ?? await _userManager.FindByNameAsync(resendEmailConfirmationVM.EmailOrUserName);
            if (user is null)
            {
                ModelState.AddModelError("", "User not found");
                return View(resendEmailConfirmationVM);
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is already confirmed");
                return View(resendEmailConfirmationVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { Area = "Identity", token, userId = user.Id }, Request.Scheme);
            string htmlMessage = $@"
<div style='font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); overflow: hidden;'>
        <div style='background-color: #007bff; color: white; padding: 15px; text-align: center;'>
            <h2 style='margin: 0;'>Cinema Reservation</h2>
        </div>
        <div style='padding: 20px;'>
            <h3 style='color: #333;'>Confirm your email address</h3>
            <p style='color: #555; line-height: 1.6;'>
                Hi <strong>{user.FirstName}</strong>,<br><br>
                Thank you for registering with <strong>Cinema Reservation</strong>.<br>
                Please click the button below to confirm your email address.
            </p>

            <div style='text-align: center; margin: 30px 0;'>
                <a href='{link}'
                   style='background-color: #007bff; color: white; padding: 12px 25px;
                          text-decoration: none; border-radius: 6px; display: inline-block;'>
                    Confirm Email
                </a>
            </div>

            <p style='font-size: 14px; color: #777;'>
                If you did not create this account, please ignore this email.
            </p>
        </div>
        <div style='background-color: #f0f0f0; text-align: center; padding: 10px; font-size: 12px; color: #999;'>
            © 2025 Cinema Reservation. All rights reserved.
        </div>
    </div>
</div>";

            await _emailSender.SendEmailAsync(user.Email, "Cinema Reservation Confirmation Mail", htmlMessage);
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login details.");
                return View(loginVM);
            }
            var user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
                    return View(loginVM);
                }
                else if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "You need to confirm your email before logging in.");
                    return View(loginVM);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View(loginVM);
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
