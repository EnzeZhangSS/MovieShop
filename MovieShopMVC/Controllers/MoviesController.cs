using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // go to movie service -> movie repository and get movie details from Movies Table
            var movieDetails = await _movieService.GetMovieDetails(id);
            return View(movieDetails);
        }

        public async Task<ActionResult> GenreMovies(int id, int pageSize = 30, int page = 1)
        {
            var pagedMovies = await _movieService.GetMoviesByPagination(id, pageSize, page);
            return View(pagedMovies);
        }

    }
}
