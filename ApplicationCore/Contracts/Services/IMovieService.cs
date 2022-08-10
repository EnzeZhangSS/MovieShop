using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTopRevenueMovies();
        Task<List<MovieCardModel>> GetTopRatingMovies();
        Task<MovieDetailsModel> GetMovieDetails(int movieId);
        Task<List<ReviewDetailsModel>> GetAllReviewsOfMovie(int movieId);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int genreId, int pageSize = 30, int page = 1);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByTitle(string title, int pageSize = 30, int page = 1);
    }
}
