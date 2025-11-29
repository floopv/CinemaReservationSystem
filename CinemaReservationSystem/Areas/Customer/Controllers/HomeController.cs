using CinemaReservationSystem.DataConnection;
using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    //[Authorize]
    public class HomeController : Controller
    {
        IRepository<Movie> _MovieRepository; //= new Repository<Brand>();

        public HomeController(IRepository<Movie> movieRepository)
        {
            _MovieRepository = movieRepository;
        }

        //private readonly ApplicationDbContext _db = new ApplicationDbContext();


        public async Task<IActionResult> Index()
        {
          var movies = await _MovieRepository.GetAllAsync();
            //var actors = _db.Actors.AsEnumerable();
            return View(movies.AsEnumerable());
        }
        public async Task<IActionResult> ViewMovie(int id)
        {
            var movie = await _MovieRepository.GetOneAsync(m=>m.Id == id , [m=>m.Category , m=>m.Cinema]);
            if(movie == null)
            {
               return NotFound();
            }
            var relatedMovies = await _MovieRepository.GetAllAsync(m => m.CategoryId
            == movie.CategoryId && m.Id != movie.Id);
            var viewModel = new MovieWithRelatedMoviesVM
            {
                Movie = movie,
                RelatedMovies = relatedMovies.AsEnumerable()
            };
            return View(viewModel);
        }
    }
}
