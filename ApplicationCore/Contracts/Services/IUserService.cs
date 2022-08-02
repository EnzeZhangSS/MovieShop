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
        Task<bool> AddFavorite(UserFavoriteRequestModel favoriteRequest);
        Task<bool> RemoveFavorite(UserFavoriteRequestModel favoriteRequest);
        Task<bool> FavoriteExists(int userid, int movieId);
        Task<PagedResultSet<MovieCardModel>> GetAllFavoritesForUser(int userid, int pageSize = 30, int page = 1);
        Task<bool> AddMovieReview(UserReviewRequestModel reviewRequest);
        Task<bool> UpdateMovieReview(UserReviewRequestModel reviewRequest);
        Task<bool> DeleteMovieReview(int userId, int movieId);
        Task<List<ReviewDetailsModel>> GetAllReviewsByUser(int userId);
        Task<bool> ReviewExists(int userId, int movieId);
        Task<ReviewDetailsModel> GetReviewDetails(int userId, int movieId);
        Task<bool> EditProfile(UserEditModel editProfileRequest, int userId);

    }
}
