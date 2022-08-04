using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Movies : ControllerBase // MVC Controller is inherited from ControllerBase
    {
        // add dependencies to Infra & AppCore just like MVC
        private readonly IMovieService _movieService;
        public Movies(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet] // without Http Request, Swagger will show an error
        [Route("top-revenue")] // always need to use Route, can also be used in MVC
        // Attribute Routing
        // MVC -> http://localhose/movies/GetTopRevenueMovies => traditional/conventional routing
        // we want, http://localhost/api/movies/top-revenue
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            // call my service
            var movies = await _movieService.GetTopRevenueMovies(); // need to convert data type

            // return the movies information in JSON format
            // ASP.NET Core automatically serializes C# Objects -> JSON Objects
            // System.Text.Json (Since .NET 3)
            // Older versions used private nuget packages "Newtonsoft.Json"

            // need to return data(.json) along with Http Status Code
            if (movies == null || !movies.Any())
            {
                // 404 status code
                return NotFound(new { errorMessage = "No Movies Found" }); // create anonnymous error message object
            }
            // 201 status code
            return Ok(movies);

        }
        [HttpGet]
        [Route("{movieId:int}")]//place holder
        public async Task<IActionResult> GetMovie(int movieId) // matching the parameter name
        {
            var movie = await _movieService.GetMovieDetails(movieId);
            if(movie== null)
            {
                return NotFound(new { errorMessage = $"No Movie Found for {movieId}"});
            }
            return Ok(movie);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetTopRevenueMovies();

            if (movies == null || !movies.Any())
            {
                // 404
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }
        [HttpGet]
        [Route("top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedmovies();

            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }
        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);

            if (movies == null)
            {
                return NotFound(new { errorMessage = "No Movies Found for this genre" });
            }

            return Ok(movies);
        }
        [HttpGet]
        [Route("{movieId:int}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int movieId)
        {
            var reviews = await _movieService.GetMovieReviews(movieId);

            if (reviews == null || !reviews.Any())
            {
                // 404
                return NotFound(new { errorMessage = "No Reviews Found For This Movie" });
            }

            return Ok(reviews);

        }
    }
}
