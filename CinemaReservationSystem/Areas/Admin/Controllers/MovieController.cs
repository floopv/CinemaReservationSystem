using CinemaReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Cinema> _cinemaRepository;
        private readonly IRepository<MovieSubImg> _movieSubImgRepository;
        private readonly IRepository<Actor> _actorRepository;
        private readonly IRepository<ActorMovie> _movieActorRepository;
        public MovieController(IRepository<Movie> movieRepository, IRepository<Category> categoryRepository, IRepository<Cinema> cinemaRepository, IRepository<MovieSubImg> movieSubImgRepository, IRepository<Actor> actorRepository, IRepository<ActorMovie> movieActorRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
            _movieSubImgRepository = movieSubImgRepository;
            _actorRepository = actorRepository;
            _movieActorRepository = movieActorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllAsync(includes: [m => m.Cinema, m => m.Category]);
            return View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateMovieVM
            {
                Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                Cinemas = (await _cinemaRepository.GetAllAsync()).ToList(),
                Actors = (await _actorRepository.GetAllAsync()).ToList(),
                SelectedActorIds = new List<int>()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieVM createMovieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error Creating Movie");
            }
            if (createMovieVM is not null)
            {
                Movie movie = new Movie
                {
                    Name = createMovieVM.Name,
                    Description = createMovieVM.Description,
                    Price = createMovieVM.Price,
                    Status = createMovieVM.Status,
                    ReleaseDate = createMovieVM.ReleaseDate,
                    CategoryId = createMovieVM.CategoryId,
                    CinemaId = createMovieVM.CinemaId
                };
                if (createMovieVM.FormMainImg is not null)
                {
                    if (createMovieVM.FormMainImg.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + createMovieVM.FormMainImg.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            createMovieVM.FormMainImg.CopyTo(stream);
                        }
                        movie.MainImg = fileName;
                    }
                }
                await _movieRepository.AddAsync(movie);
                await _movieRepository.CommitAsync();
                foreach (var subImg in createMovieVM.FormSubImgs)
                {
                    if (subImg is not null)
                    {
                        if (subImg.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + "_" + subImg.FileName;
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies\\Movies_SubImgs", fileName);
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                subImg.CopyTo(stream);
                            }
                            MovieSubImg movieSubImg = new MovieSubImg
                            {
                                SubImg = fileName,
                                MovieId = movie.Id
                            };
                            await _movieSubImgRepository.AddAsync(movieSubImg);
                            await _movieSubImgRepository.CommitAsync();
                        }
                    }
                }
                foreach (var actor in createMovieVM.SelectedActorIds)
                {
                    if (actor >= 0)
                    {
                        ActorMovie movieActors = new ActorMovie
                        {
                            MoviesId = movie.Id,
                            ActorsId = actor
                        };
                        await _movieActorRepository.AddAsync(movieActors);
                        await _movieActorRepository.CommitAsync();
                    }
                }
            }
            else
                ModelState.AddModelError(string.Empty, "Error Creating Movie");

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var movie = await _movieRepository.GetOneAsync(c => c.Id == id, includes: [m => m.ActorMovies, m => m.SubImgs]);
            if (movie is null)
            {
                return NotFound();
            }
            //var updateMovieVM = movie.Adapt<UpdateMovieVM>();
            UpdateMovieVM updateMovieVM = new UpdateMovieVM
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                Status = movie.Status,
                ReleaseDate = movie.ReleaseDate,
                MainImg = movie.MainImg,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,
                Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                Cinemas = (await _cinemaRepository.GetAllAsync()).ToList(),
                Actors = (await _actorRepository.GetAllAsync()).ToList(),
                SelectedActorIds = movie.ActorMovies?.Select(am => am.ActorsId).ToList() ?? new List<int>(),
                MovieSubImgs = movie.SubImgs?.ToList()
            };
            return View(updateMovieVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateMovieVM updateMovieVM)
        {
            if (!ModelState.IsValid)
            {
                updateMovieVM.Categories = (await _categoryRepository.GetAllAsync()).ToList();
                updateMovieVM.Cinemas = (await _cinemaRepository.GetAllAsync()).ToList();
                updateMovieVM.Actors = (await _actorRepository.GetAllAsync()).ToList();
                updateMovieVM.MovieSubImgs = (await _movieSubImgRepository.GetAllAsync(m => m.MovieId == updateMovieVM.Id)).ToList();
                ModelState.AddModelError(string.Empty, "Error Updating Movie");
                return View(updateMovieVM);
            }
            var movie = await _movieRepository.GetOneAsync(c => c.Id == updateMovieVM.Id, includes: [m => m.ActorMovies, m => m.SubImgs]);
            var movieInDb = await _movieRepository.GetOneAsync(c => c.Id == updateMovieVM.Id, includes: [m => m.ActorMovies, m => m.SubImgs] , asNoTracking:true);
            if (movie is null)
            {
                return NotFound();
            }
            movie.Name = updateMovieVM.Name;
            movie.Description = updateMovieVM.Description;
            movie.Price = updateMovieVM.Price;
            movie.Status = updateMovieVM.Status;
            movie.ReleaseDate = updateMovieVM.ReleaseDate;
            movie.CategoryId = updateMovieVM.CategoryId;
            movie.CinemaId = updateMovieVM.CinemaId;
            _movieRepository.Update(movie);
            await _movieRepository.CommitAsync();

            if (updateMovieVM.FormMainImg is not null)
            {
                if (updateMovieVM.FormMainImg.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + updateMovieVM.FormMainImg.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        updateMovieVM.FormMainImg.CopyTo(stream);
                    }
                    movie.MainImg = fileName;
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies", movieInDb.MainImg);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                else
                {
                    movie.MainImg = movieInDb.MainImg;

                }
            }
            _movieRepository.Update(movie);
            if(updateMovieVM.FormSubImgs is not null)
            {
                foreach (var subImg in updateMovieVM.FormSubImgs)
            {
                if (subImg is not null)
                {
                    if (subImg.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + subImg.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies\\Movies_SubImgs", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            subImg.CopyTo(stream);
                        }
                        MovieSubImg movieSubImg = new MovieSubImg
                        {
                            SubImg = fileName,
                            MovieId = movie.Id
                        };
                         await _movieSubImgRepository.AddAsync(movieSubImg);
                    }
                }

            }
                await _movieSubImgRepository.CommitAsync();

            }

            if (updateMovieVM.SelectedActorIds != null)
            {
                updateMovieVM.SelectedActorIds = updateMovieVM.SelectedActorIds.Distinct().ToList();

                var movieActorList = await _movieActorRepository.GetAllAsync(
                    expression: ma => ma.MoviesId == movie.Id);
                var actorsInDb = movieActorList.Select(ma => ma.ActorsId).ToList();

                var actorsToRemove = actorsInDb.Except(updateMovieVM.SelectedActorIds).ToList();
                foreach (var actorId in actorsToRemove)
                {
                    _movieActorRepository.Delete(new ActorMovie
                    {
                        MoviesId = movie.Id,
                        ActorsId = actorId
                    });
                }
                await _movieActorRepository.CommitAsync();


                var actorsToAdd = updateMovieVM.SelectedActorIds.Except(actorsInDb).ToList();
                foreach (var actorId in actorsToAdd)
                {
                    await _movieActorRepository.AddAsync(new ActorMovie
                    {
                        MoviesId = movie.Id,
                        ActorsId = actorId
                    });
                }
                await _movieActorRepository.CommitAsync();

            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteSubImage(int? movieId, string? img)
        {
            if (movieId is not null && img is not null)
            {
                var movieSubImg = await _movieSubImgRepository.GetOneAsync(expression: msi => msi.MovieId == movieId && msi.SubImg == img);
                _movieSubImgRepository.Delete(movieSubImg);
                await _movieSubImgRepository.CommitAsync();
            }
            else
                return RedirectToAction(nameof(Index));

            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies_SubImgs", img);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            return RedirectToAction(nameof(Update), nameof(Movie), new { id = movieId });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetOneAsync(expression: m => m.Id == id,
                includes: [m => m.SubImgs]);
            if (movie == null)
                return RedirectToAction(nameof(Index));
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", movie.MainImg);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            if (movie.SubImgs.Count > 0)
            {
                foreach (var subImg in movie.SubImgs)
                {
                    var subImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imgs\\Movies_SubImgs", subImg.SubImg);
                    if (System.IO.File.Exists(subImgPath))
                    {
                        System.IO.File.Delete(subImgPath);
                    }
                }
            }
            _movieRepository.Delete(movie);
            await _movieRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
