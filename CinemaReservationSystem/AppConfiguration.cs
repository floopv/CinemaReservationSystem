using CinemaReservationSystem.DataConnection;
using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using CinemaReservationSystem.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationSystem
{
    public static class AppConfiguration
    {
        public static void Config(this IServiceCollection services , string connectionString)
        {
            services.AddTransient<IEmailSender , EmailSender>();
            services.AddScoped<IRepository<Actor>, Repository<Actor>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            services.AddScoped<IRepository<MovieSubImg>, Repository<MovieSubImg>>();
            services.AddScoped<IRepository<ActorMovie>, Repository<ActorMovie>>();
            services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
