using ApplicationCore.Entities;
using ApplicationCore.Contracts.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;

        public UserRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }

        public async Task<Favorite> AddFavorite(Favorite favorite)
        {
            _movieShopDbContext.Favorites.Add(favorite);
            await _movieShopDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<Purchase> AddPurchase(Purchase purchase)
        {
            _movieShopDbContext.Purchases.Add(purchase);
            await _movieShopDbContext.SaveChangesAsync();
            return purchase;
        }

        public async Task<Review> AddReview(Review review)
        {
            _movieShopDbContext.Reviews.Add(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<User> AddUser(User user)
        {
            _movieShopDbContext.Users.Add(user);
            await _movieShopDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetById(int id)
        {
            var userDetails = await _movieShopDbContext.Users
               .Include(u => u.Purchases)
               .Include(u => u.Reviews)
               .Include(u => u.Favorites)
               .FirstOrDefaultAsync(u => u.Id == id);
            return userDetails;
        }

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
        {
            var favoriteDetails = await _movieShopDbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);
            return favoriteDetails;
        }

        public async Task<Purchase> GetPurchaseById(int userId, int movieId)
        {
            var purchaseDetails = await _movieShopDbContext.Purchases
                .FirstOrDefaultAsync(p => p.UserId == userId && p.MovieId == movieId);
            return purchaseDetails;
        }

        public async Task<Review> GetReviewById(int userId, int movieId)
        {
            var reviewDetails = await _movieShopDbContext.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
            return reviewDetails;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _movieShopDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}