

using CinemaReservationSystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActorReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role} , {ConstantData.Employee_Role}")]

    public class ActorController : Controller
    {

        private readonly IRepository<Actor> _actorRepository;

        public ActorController(IRepository<Actor> actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var actors = await _actorRepository.GetAllAsync();
            return View(actors.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateActorVM createActorVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                return View(createActorVM);
            }
            var actor = createActorVM.Adapt<Actor>();
            if (createActorVM.ImgFile is not null)
            {
                if (createActorVM.ImgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + createActorVM.ImgFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Actors", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        createActorVM.ImgFile.CopyTo(stream);
                    }
                    actor.Img = fileName;
                }

                await _actorRepository.AddAsync(actor);
                await _actorRepository.CommitAsync();

            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var actor = await _actorRepository.GetOneAsync(c => c.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            var updateActorVM = actor.Adapt<UpdateActorVM>();
            updateActorVM.Img = actor.Img;
            return View(updateActorVM);
        }
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        [HttpPost]
        public async Task<IActionResult> Update(UpdateActorVM updateActorVM)
        {
            var actorInDb = await _actorRepository.GetOneAsync(c => c.Id == updateActorVM.Id, asNoTracking: true);
            updateActorVM.Img = actorInDb.Img;
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                updateActorVM.Img = actorInDb.Img;
                return View(updateActorVM);
            }
            var actor = updateActorVM.Adapt<Actor>();
            if (actor is null)
            {
                return NotFound();
            }
            if (updateActorVM.ImgFile is not null)
            {
                if (updateActorVM.ImgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + updateActorVM.ImgFile.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Actors", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        updateActorVM.ImgFile.CopyTo(stream);
                    }
                    actor.Img = fileName;
                }
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Actors", actorInDb.Img);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }
            else
            {
                actor.Img = actorInDb.Img;
            }
            _actorRepository.Update(actor);
            await _actorRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorRepository.GetOneAsync(c => c.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            if (actor.Img is not null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imgs/Actors", actor.Img);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            _actorRepository.Delete(actor);
            await _actorRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
