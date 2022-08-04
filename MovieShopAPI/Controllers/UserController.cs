using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Infra;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // need to be authorized
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        public UserController(IUserService userService,ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }
        [HttpGet]
        [Route("details/{userId:int}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var user = await _userService.UserDetails(userId);
            if(user == null)
            {
                return NotFound(new {errorMessage="No User is found"});
            }
            return Ok(user);
        }
        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> PurchaseMovie([FromBody]PurchaseRequestModel model)
        {
            if (await _userService.PurchaseMovie(model, model.UserId))
            {
                return Ok(model);
            }
            return Conflict(new { errorMessage = "Movie is already purchased" });
        }
        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> AddFavorite([FromBody]FavoriteRequestModel model)
        {
            if (await _userService.AddFavorite(model))
            {
                return Ok(model);
            }
            return Conflict(new { errorMessage = "Movie is already added to favorites" });
        }
        [HttpPost]
        [Route("un-favorite")]
        public async Task<IActionResult> RemoveFavorite([FromBody]FavoriteRequestModel model)
        {
            if (await _userService.RemoveFavorite(model))
            {
                return Ok(model);
            }
            return Conflict(new { errorMessage = "Movie is not in favorites" });
        }
        [HttpGet]
        [Route("check-movie-favorite/{movieId:int}")]
        public async Task<IActionResult> CheckMovieFavorite(int movieId)
        {// check if movie is favorited by authenticated user
            if(await _userService.FavoriteExists(_currentUser.UserId, movieId))
            {
                return Ok();
            }
            return NotFound(new { errorMessage = "Movie is not in favorites" });
        }

        [HttpPost]
        [Route("add-review")]
        public async Task<IActionResult> AddReview([FromBody]ReviewRequestModel model)
        {
            if(await _userService.AddMovieReview(model))
            {
                return Ok(model);
            }
            return Conflict(new { errorMessage = "User already added review for this movie" });
        }

        [HttpPut]
        [Route("edit-review")]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewRequestModel model)
        {
            await _userService.UpdateMovieReview(model);
            return Ok(model);
        }

        [HttpDelete]
        [Route("delete-review/{movieId:int}")]
        public async Task<IActionResult> DeleteReview([FromBody] ReviewRequestModel model)
        {
            if(await _userService.DeleteMovieReview(model.UserId, model.MovieId))
            {
                return Ok(model);
            }
            return NotFound(new {errorMessage = "User has no review for this movie"});
        }
        // after delete review
        [HttpGet]
        [Route("Purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser()
        {
            // need to get the userId from the Token, using HttpContext
            var userId = _currentUser.UserId;
            var purchases = await _userService.GetAllPurchasesForUser(userId);
            return Ok(purchases);
        }
        [HttpGet]
        [Route("purchase-details/{movieId:int}")]
        public async Task<IActionResult> GetPurchaseDetails(int movieId)
        {
            var purchaseDetail = await _userService.GetPurchasesDetails(_currentUser.UserId, movieId);
            if(purchaseDetail == null)
            {
                return NotFound(new { errorMessage = "User has not purchased this movie" });
            }
            return Ok(purchaseDetail);
        }

        [HttpGet]
        [Route("check-movie-purchased/{movieId:int}")]
        public async Task<IActionResult> CheckMoviePurchased(int movieId)
        {
            if(await _userService.IsMoviePurchased(new PurchaseRequestModel {MovieId = movieId}, _currentUser.UserId))
            {
                return Ok();
            }
            return NotFound(new { errorMessage = "User has not purchased this movie" });
        }

        [HttpGet]
        [Route("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var favorites = await _userService.GetAllFavoritesForUser(_currentUser.UserId);
            return Ok(favorites);
        }

        [HttpGet]
        [Route("movie-reviews")]
        public async Task<IActionResult> GetMovieReviews()
        {
            var reviews = await _userService.GetAllReviewsByUser(_currentUser.UserId);
            return Ok(reviews);
        }
    }
}
