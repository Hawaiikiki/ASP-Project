using ApplicationCore.ServiceContracts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Models;
using System.Diagnostics;

namespace MovieShopMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;

        // we need to tell our framework what we want to pass as parameter
        public HomeController(ILogger<HomeController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
            
        }

        // Action methods inside the controller
        // executed with GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // controllers call services which are gonna call repositories


            var movieCards = await _movieService.GetTopRevenueMovies();
            return View(movieCards);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TopRatedMovies()
        {
            return View(); // we can manually add the name of the view inside ()
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}