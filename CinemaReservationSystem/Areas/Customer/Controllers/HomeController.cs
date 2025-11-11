using CinemaReservationSystem.DataConnection;
using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        //IRepository<Actor> _actorRepository; //= new Repository<Brand>();
        //public HomeController(IRepository<Actor> actorRepository)
        //{
        //    _actorRepository = actorRepository;
        //}
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        [Area("Customer")]
        public async Task<IActionResult> Index()
        {
          //var actors = await _actorRepository.GetAllAsync();
            var actors = _db.Actors.AsEnumerable();
            return View(actors);
        }
    }
}
