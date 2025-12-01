using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaReservationSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    //[Authorize]
    public class CartController : Controller
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Movie> _movieRepository;

        public CartController(IRepository<Cart> cartRepository, UserManager<ApplicationUser> userManager, IRepository<Movie> movieRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = await _cartRepository.GetAllAsync(c => c.ApplicationUserId == user.Id, [m=>m.Movie]);
            return View(cartItems);
        }
        public async Task<IActionResult> AddToCart(int MovieId , int Count)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var movie = await _movieRepository.GetOneAsync(m => m.Id == MovieId);
            if (movie == null)
            {
                return NotFound();
            }
            if (Count <= 0)
            {
                return RedirectToAction("ViewMovie" , "Home" , new {id = MovieId});
            }
            var existingCartItem = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.MovieId == MovieId);
            if (existingCartItem != null)
            {
                existingCartItem.Count += Count;
                 _cartRepository.Update(existingCartItem);
            }
            else
            {
                var cart = new Cart()
                {
                    ApplicationUserId = user.Id,
                    MovieId = MovieId,
                    Count = Count,
                    Price = movie.Price
                };
                await _cartRepository.AddAsync(cart);
            }
            await _cartRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> IncrementMovie(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.MovieId == movieId);
            if (cartItems != null)
            {
                cartItems.Count += 1;
                _cartRepository.Update(cartItems);
                await _cartRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DecrementMovie(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.MovieId == movieId);
            if (cartItems != null)
            {
                if (cartItems.Count <= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                cartItems.Count--;
                _cartRepository.Update(cartItems);
                await _cartRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.MovieId == movieId);
            if (cartItems != null)
            {
                _cartRepository.Delete(cartItems);
                await _cartRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
