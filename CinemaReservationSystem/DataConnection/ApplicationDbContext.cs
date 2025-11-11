using CinemaReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationSystem.DataConnection
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<MovieSubImg> MovieSubImgs { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer ("Data Source=FLOOPV\\SQLEXPRESS;initial catalog = CinemaReservationSystem ;Integrated Security=True;" +
        //        "Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;" +
        //        "Multi Subnet Failover=False");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Actor>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Category>().
                HasKey(c => c.Id);
            modelBuilder.Entity<Cinema>().
                HasKey(c => c.Id);
            modelBuilder.Entity<MovieSubImg>().
                HasKey(m => new
                {
                    m.MovieId,
                    m.SubImg
                });
            modelBuilder.Entity<Movie>().
                HasKey(m => m.Id);
        }
    }
}
