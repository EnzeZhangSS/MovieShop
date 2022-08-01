using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IUserService
    {
        Task<bool> PurchaseMovie(UserPurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(int userId, int movieId);
        Task<PagedResultSet<MovieCardModel>> GetAllPurchasesForUser(int userid, int pageSize = 30, int page = 1);
        Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId);

        /*
        Task AddFavorite(UserFavoriteRequestModel favoriteRequest);
        Task RemoveFavorite(UserFavoriteRequestModel favoriteRequest);
        Task FavoriteExists(int id, int movieId);
        Task GetAllFavoritesForUser(int id);
        Task AddMovieReview(UserReviewRequestModel reviewRequest);
        Task UpdateMovieReview(UserReviewRequestModel reviewRequest);
        Task DeleteMovieReview(int userId, int movieId);
        Task GetAllReviewsByUser(int id);
        */
    }
}
