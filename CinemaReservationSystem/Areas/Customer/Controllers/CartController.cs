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
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<ApplicationUserPromotionCode> _applicationUserPromotionCodeRepository;

        public CartController(IRepository<Cart> cartRepository, UserManager<ApplicationUser> userManager, IRepository<Movie> movieRepository, IRepository<Promotion> promotionRepository, IRepository<ApplicationUserPromotionCode> applicationUserPromotionCodeRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
            _promotionRepository = promotionRepository;
            _applicationUserPromotionCodeRepository = applicationUserPromotionCodeRepository;
        }

        public async Task<IActionResult> Index(string? code = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = await _cartRepository.GetAllAsync(c => c.ApplicationUserId == user.Id, [m=>m.Movie]);
            if (code != null)
            {
                var promotion = await _promotionRepository.GetOneAsync(p => p.Code == code);
                if (promotion != null)
                {
                    if(promotion.IsValid && promotion.ExpiryDate > DateTime.UtcNow && promotion.MaxUsage > 0)
                    {
                       var movieInCart = cartItems.FirstOrDefault(c => c.MovieId == promotion.MovieId);
                        if (movieInCart != null)
                        {
                            var alreadyUsed = await _applicationUserPromotionCodeRepository.GetOneAsync(apc => apc.ApplicationUserId == user.Id && apc.PromotionCodeId == promotion.Id);
                            if (alreadyUsed != null)
                            {
                                TempData["Error"] = "You have already used this promotion code.";
                                return View(cartItems);
                            }
                            movieInCart.Price -= movieInCart.Price * ( promotion.Discount / 100);
                            promotion.MaxUsage--;
                            if (promotion.MaxUsage == 0)
                            {
                                promotion.IsValid = false;
                            }
                            _promotionRepository.Update(promotion);
                            await _applicationUserPromotionCodeRepository.AddAsync(new ApplicationUserPromotionCode()
                            {
                                ApplicationUserId = user.Id ,
                                PromotionCodeId = promotion.Id
                            });
                            await _promotionRepository.CommitAsync();
                            TempData["Success"] = "Promotion code applied successfully.";
                        }
                        else
                        {
                            TempData["Error"] = "Promotion code is not applicable to any movie in your cart.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Promotion code is expired or invalid.";
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid promotion code.";
                }
            }
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
        public async Task<IActionResult> Payment()
{
    var options = new SessionCreateOptions
    {
        PaymentMethodTypes = new List<string> { "card" },
        LineItems = new List<SessionLineItemOptions>(),
        Mode = "payment",
        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/success",
        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/cancel",
    };
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        return RedirectToAction("Login", "Account", new { area = "Identity" });
    }
    var cartItems = await _cartRepository.GetAllAsync(c => c.ApplicationUserId == user.Id, [m => m.Movie]);
    foreach (var item in cartItems)
    {
        var sessionLineItems = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "EGP",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.Movie.Name,
                    Description = item.Movie.Description,
                },
                UnitAmount = (long)item.Price * 100,
            },
            Quantity = item.Count,
        };
        options.LineItems.Add(sessionLineItems);
    }
    var service = new SessionService();
    var session = service.Create(options);
    return Redirect(session.Url);
}
    }
}
