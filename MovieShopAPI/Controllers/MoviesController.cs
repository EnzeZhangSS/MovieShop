using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchMovieByTitle(string title, int pageSize = 30, int page = 1)
        {
            var pagedMovies = await _movieService.GetMoviesByTitle(title, pageSize, page);

            if (pagedMovies == null)
            {
                return NotFound(new { errorMessage = "No Movie Found for This Title" });
            }
            return Ok(pagedMovies);

        }

        [HttpGet]
        [Route("top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatingMovies();

            if (movies == null || !movies.Any())
            {
                //404
                return NotFound(new { errorMessage = "No Movie Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("top-grossing")]
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            var movies = await _movieService.GetTopRevenueMovies();

            if(movies == null || !movies.Any())
            {
                //404
                return NotFound( new { errorMessage = "No Movie Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { errorMessage = $"No Movie Found for {id}" });
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<ActionResult> GenreMovies(int genreId, int pageSize = 30, int pageIndex = 1)
        {
            var pagedMovies = await _movieService.GetMoviesByPagination(genreId, pageSize, pageIndex);
            if (pagedMovies == null)
            {
                return NotFound(new { errorMessage = "No Movie Found for This Genre" });
            }
            return Ok(pagedMovies);

        }


        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMoviesReviewsById(int id)
        {

            var reviews = await _movieService.GetAllReviewsOfMovie(id);
            if (reviews == null)
            {
                //404
                return NotFound(new { errorMessage = "No Review Found for This Movie" });
            }

            return Ok(reviews);
        }

    }
}
