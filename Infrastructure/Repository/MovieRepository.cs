﻿using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public MovieRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }
        public async Task<Movie> GetById(int id)
        {
            // select * from movie where id = 1 join genre, cast, moviegerne, moviecast
            var movieDetails = await _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .Include(m => m.PurchasesOfMovie)
                .Include(m => m.ReviewsOfMovie)
                .Include(m => m.FavoritesOfMovie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieDetails == null)
            {
                return movieDetails;
            }

            movieDetails.Rating = await GetMovieRatingById(id);

            return movieDetails;
        }

        public async Task<decimal> GetMovieRatingById(int id)
        {
            var rating = 0m;
            var hit = await _movieShopDbContext.Reviews.Where(h => h.MovieId == id).CountAsync();
            if (hit != 0)
            {
                rating = await _movieShopDbContext.Reviews.Where(r => r.MovieId == id).AverageAsync(r => r.Rating);
            }
            return rating;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int page = 1)
        {
            // get total row count
            var totalMoviesCountOfGenre = await _movieShopDbContext.MovieGenres.Where(g => g.GenreId == genreId).CountAsync();
            if (totalMoviesCountOfGenre == 0)
            {
                throw new Exception("No Movies found for this genre");
            }

            // get the actual data
            var movies = await _movieShopDbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(g => g.Movie).OrderByDescending(m => m.Movie.Revenue)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalMoviesCountOfGenre);
            return pagedMovies;
        }

        public async Task<List<Movie>> GetTop30HighestRevenueMovies()
        {

            // call the database with EF Core and get the data
            // use MovieShopDbContext and Movies DbSet
            // select top 30 * from Movies order by Revenue
            // corresponding LINQ Query

            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Revenue)
                .Select(m => new Movie { Id = m.Id, Title = m.Title, PosterUrl = m.PosterUrl })
                .Take(30).ToListAsync();
            return movies;
        }

        public async Task<List<Movie>> GetTop30RatedMovies()
        {
            var idList = await _movieShopDbContext.Reviews.GroupBy(r => new Review { MovieId = r.MovieId}).OrderByDescending(r => r.Average(r => r.Rating))
                .Select(r => new {ID = r.First().MovieId})
                .Take(30).ToListAsync();
            /*
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => GetMovieRatingById(m.Id))
                .Select(m => new Movie { Id = m.Id, Title = m.Title, PosterUrl = m.PosterUrl })
                .Take(30).ToListAsync();
            */
            var movies = new List<Movie>();
            foreach(var i in idList)
            {
                movies.Add(await _movieShopDbContext.Movies.FirstOrDefaultAsync(m => m.Id == i.ID));
            }

            return movies;

        }

        public async Task<List<Review>> GetAllReviews(int movieId)
        {
            var totalReviewsOfMovie = await _movieShopDbContext.Reviews.Where(r => r.MovieId == movieId).CountAsync();
            if (totalReviewsOfMovie == 0)
            {
                //throw new Exception("You didn't have any review yet.");
                return null;
            }

            var reviews = await _movieShopDbContext.Reviews.Where(r => r.MovieId == movieId).ToListAsync();

            return reviews;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByTitlePagination(string title, int pageSize = 30, int page = 1)
        {
            var totalMoviesCountOfTitle = await _movieShopDbContext.Movies.Where(t => t.Title.Contains(title)).CountAsync();
            if (totalMoviesCountOfTitle == 0)
            {
                throw new Exception("No Movies found for this title");
            }

            // get the actual data
            var movies = await _movieShopDbContext.Movies.Where(t => t.Title.Contains(title)).OrderByDescending(m => m.Revenue)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalMoviesCountOfTitle);
            return pagedMovies;
        }
    }
}
