using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Infra;

namespace MovieShopMVC.Controllers
{
    [Authorize]// check if authenticated or not
               // if not logged in, it will redirect to login page by AddAuthentication details in program.cs
    public class UserController : Controller 
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        public UserController(ICurrentUser currentUser, IUserService userService, IMovieService movieService)
        {
            _currentUser = currentUser;
            _userService = userService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            // get all the movies purchased by user (userId)
            // httpcontext.user.claims -> then call the database -> then get the info to the view
            var userId = _currentUser.UserId;
            var purchases = await _userService.GetAllPurchasesForUser(userId);
            return View(purchases);
        }
        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            // get all the favorite movies
            var userId = _currentUser.UserId;
            var favorites = await _userService.GetAllFavoritesForUser(userId);
            return View(favorites);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditModel model)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyMovie(PurchaseRequestModel model)
        {
            // when user click on Purchase, shows purchase confirmation pop up
            var userId = _currentUser.UserId;
            await _userService.PurchaseMovie(model, userId);
            return LocalRedirect("~/Movies/Details/"+model.MovieId);
        }
        [HttpPost]
        public async Task<IActionResult> FavoriteMovies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewRequestModel model)
        {
            // when user click on Review, shows review confirmation pop up
            await _userService.AddMovieReview(model);
            return LocalRedirect("~/Movies/Details/"+model.MovieId);
        }
    }
}
