using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using CinemaReservationSystem.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
           var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }
        [HttpGet]
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM createCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "There were some errors. Please correct them and try again.";
                return View(createCategoryVM);
            }
            if (createCategoryVM is not null)
            {
                var category = createCategoryVM.Adapt<Category>();
                await _categoryRepository.AddAsync(category);
                await _categoryRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
