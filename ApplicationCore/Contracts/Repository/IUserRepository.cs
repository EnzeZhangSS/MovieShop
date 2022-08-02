using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> AddUser(User user);
        Task<User> GetById(int id);
        Task<Purchase> AddPurchase(Purchase purchase);
        Task<Purchase> GetPurchaseById(int userId, int movieId);
        Task<Review> AddReview(Review review);
        Task<Review> GetReviewById(int userId, int movieId);
        Task<Favorite> AddFavorite(Favorite favorite);
        Task<Favorite> GetFavoriteById(int userId, int movieId);
        Task<bool> RemoveFavorite(Favorite favorite);
        Task<bool> DeleteReview(int userId, int movieId);
        Task<bool> UpdateReview(Review review);
        Task<PagedResultSet<Movie>> GetAllFavoritesPagination(int userid, int pageSize = 30, int page = 1);
        Task<List<Review>> GetAllReviews(int userid);
        Task<bool> EditUserProfile(User editProfile);


    }
}