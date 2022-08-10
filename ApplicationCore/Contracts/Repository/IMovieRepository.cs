using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Contracts.Repository
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetTop30HighestRevenueMovies();
        Task<decimal> GetMovieRatingById(int id);
        Task<List<Movie>> GetTop30RatedMovies();
        Task<Movie> GetById(int id);
        Task<PagedResultSet<Movie>> GetMoviesByTitlePagination(string title, int pageSize = 30, int page = 1);
        Task<PagedResultSet<Movie>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int page = 1);
        Task<List<Review>> GetAllReviews(int movieId);
    }
}
