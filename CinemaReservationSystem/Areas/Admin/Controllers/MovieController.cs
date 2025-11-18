using Microsoft.AspNetCore.Mvc;
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
            var movies = await _movieRepository.GetAllAsync(includes: [m=>m.Cinema,m=>m.Category]);
            return View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateMovieVM
            {
                Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                Cinemas = (await _cinemaRepository.GetAllAsync()).ToList(),
                Actors = (await _actorRepository.GetAllAsync()).ToList() ,
                SelectedActorIds = new List<int>()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieVM createMovieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty , "Error Creating Movie");
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
                    if(createMovieVM.FormMainImg.Length > 0)
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
                        if(subImg.Length> 0)
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
                            await _movieSubImgRepository.CommitAsync() ;
                        }
                    }
                }
                foreach(var actor in createMovieVM.SelectedActorIds)
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
        public async Task<IActionResult> Update(int id)
        {
            var movie = await _movieRepository.GetOneAsync(c => c.Id == id , includes: [m=>m.ActorMovies]);
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
                SelectedActorIds = movie.ActorMovies?.Select(am => am.ActorsId).ToList(),
                MovieSubImgs = movie.SubImgs?.ToList()
            };
            return View(updateMovieVM);
        }
    }
}
