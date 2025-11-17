

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {

        private readonly IRepository<Cinema> _cinemaRepository;

        public CinemaController(IRepository<Cinema> cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            return View(cinemas.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCinemaVM createCinemaVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                return View(createCinemaVM);
            }
            var cinema = createCinemaVM.Adapt<Cinema>();
            if (createCinemaVM.ImgFile is not null)
            {
                if (createCinemaVM.ImgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + createCinemaVM.ImgFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Cinemas", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        createCinemaVM.ImgFile.CopyTo(stream);
                    }
                    cinema.Img = fileName;
                }

                await _cinemaRepository.AddAsync(cinema);
                await _cinemaRepository.CommitAsync();

            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var cinema = await _cinemaRepository.GetOneAsync(c => c.Id == id);
            if (cinema is null)
            {
                return NotFound();
            }
            var updateCinemaVM = cinema.Adapt<UpdateCinemaVM>();
            updateCinemaVM.Img = cinema.Img;
            return View(updateCinemaVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCinemaVM updateCinemaVM)
        {
            var cinemaInDb = await _cinemaRepository.GetOneAsync(c => c.Id == updateCinemaVM.Id , asNoTracking:true);
            updateCinemaVM.Img = cinemaInDb.Img;
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                updateCinemaVM.Img = cinemaInDb.Img;
                return View(updateCinemaVM);
            }
            var cinema = updateCinemaVM.Adapt<Cinema>();
            if (cinema is null)
            {
                return NotFound();
            }
            if (updateCinemaVM.ImgFile is not null)
            {
                if (updateCinemaVM.ImgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + updateCinemaVM.ImgFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Cinemas", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        updateCinemaVM.ImgFile.CopyTo(stream);
                    }
                    cinema.Img = fileName;
                }
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Cinemas", cinemaInDb.Img);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }
            else
            {
                cinema.Img = cinemaInDb.Img;
            }
            _cinemaRepository.Update(cinema);
            await _cinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index)) ;
        }
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaRepository.GetOneAsync(c => c.Id == id);
            if (cinema is null)
            {
                return NotFound();
            }
            if (cinema.Img is not null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Cinemas", cinema.Img);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            _cinemaRepository.Delete(cinema);
            await _cinemaRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
