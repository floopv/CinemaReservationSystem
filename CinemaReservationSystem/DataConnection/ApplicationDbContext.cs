using CinemaReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using CinemaReservationSystem.ViewModel;

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
        public DbSet<ActorMovie> ActorMovie { get; set; }
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
            modelBuilder.Entity<ActorMovie>()
       .HasKey(ma => new { ma.MoviesId, ma.ActorsId });
            modelBuilder.Entity<ActorMovie>()
       .HasOne(am => am.Movie)
       .WithMany(m => m.ActorMovies)
       .HasForeignKey(am => am.MoviesId);

            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.ActorMovies)
                .HasForeignKey(am => am.ActorsId);
        }
    }
}
