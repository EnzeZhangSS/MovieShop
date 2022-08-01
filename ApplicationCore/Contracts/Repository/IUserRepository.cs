using ApplicationCore.Entities;
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

    }
}