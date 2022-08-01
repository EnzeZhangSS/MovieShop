﻿using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Infra;
using ApplicationCore.Models;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IGenreRepository _genreRepository;


        public MoviesController(IMovieService movieService, IGenreRepository genreRepository)
        {
            _movieService = movieService;
            _genreRepository = genreRepository;

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
            ViewData["Title"] = _genreRepository.GetById(id).Result.Name;
            return View(pagedMovies);
        }

    }
}
