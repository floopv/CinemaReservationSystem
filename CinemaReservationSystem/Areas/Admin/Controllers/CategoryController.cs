using CinemaReservationSystem.Models;
using CinemaReservationSystem.Repos;
using CinemaReservationSystem.Utilities;
using CinemaReservationSystem.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role} , {ConstantData.Employee_Role}")]

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
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        [HttpGet]
        public async Task<IActionResult> Update (int id)
        {
            var category =  await _categoryRepository.GetOneAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            var updateCategoryVM = category.Adapt<UpdateCategoryVM>();
            return View(updateCategoryVM);
        }
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVM updateCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "There were some errors. Please correct them and try again.";
                return View(updateCategoryVM);
            }
            var category = await _categoryRepository.GetOneAsync(c => c.Id == updateCategoryVM.Id);
            if (category is null)
            {
                return NotFound();
            }
            category = updateCategoryVM.Adapt(category);
            _categoryRepository.Update(category);
            await _categoryRepository.CommitAsync();
            return RedirectToAction(nameof(Index)) ;
        }
        [Authorize(Roles = $"{ConstantData.Super_Admin_Role} , {ConstantData.Admin_Role}")]

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetOneAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            _categoryRepository.Delete(category);
            await _categoryRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
