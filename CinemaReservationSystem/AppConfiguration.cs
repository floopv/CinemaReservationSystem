using CinemaReservationSystem.DataConnection;
using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationSystem
{
    public static class AppConfiguration
    {
        public static void Config(this IServiceCollection services , string connectionString)
        {
            services.AddScoped<IRepository<Actor>, Repository<Actor>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            services.AddScoped<IRepository<MovieSubImg>, Repository<MovieSubImg>>();
            services.AddScoped<IRepository<ActorMovie>, Repository<ActorMovie>>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
