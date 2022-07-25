using ApplicationCore.Contracts.Services;
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
        private readonly IGenreService _genreService;

        public HomeController(ILogger<HomeController> logger, IMovieService movieService, IGenreService genreService)
        {
            _logger = logger;
            _movieService = movieService;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movieCards = await _movieService.GetTopRevenueMovies();
            return View(movieCards);
        }

        [HttpGet]
        public async Task<IActionResult> AllGenres()
        {
            var AllGenres = await _genreService.GetAllGenres();
            return View(AllGenres);
        }


        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TopRatedMovies()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}