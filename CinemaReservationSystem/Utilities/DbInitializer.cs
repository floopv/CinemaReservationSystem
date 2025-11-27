using CinemaReservationSystem.DataConnection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationSystem.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DbInitializer> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(
            ApplicationDbContext context,
            ILogger<DbInitializer> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
                if (!_roleManager.Roles.Any())
                {
                    _roleManager.CreateAsync(new IdentityRole(ConstantData.User_Role)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(ConstantData.Admin_Role)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(ConstantData.Super_Admin_Role)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(ConstantData.Employee_Role)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new ApplicationUser()
                    {
                        UserName = "SuperAdmin",
                        Email = "superAdmin@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "Super",
                        LastName = "Admin"
                    } , "Abab1010!").GetAwaiter().GetResult();
                    var user = _userManager.FindByEmailAsync("superAdmin@gmail.com").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user,ConstantData.Super_Admin_Role).GetAwaiter().GetResult();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
