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
    [Authorize]
    public class HomeController : Controller
    {
        IRepository<Actor> _actorRepository; //= new Repository<Brand>();
        public HomeController(IRepository<Actor> actorRepository)
        {
            _actorRepository = actorRepository;
        }
        //private readonly ApplicationDbContext _db = new ApplicationDbContext();

        
        public async Task<IActionResult> Index()
        {
          var actors = await _actorRepository.GetAllAsync();
            //var actors = _db.Actors.AsEnumerable();
            return View(actors.AsEnumerable());
        }
    }
}
