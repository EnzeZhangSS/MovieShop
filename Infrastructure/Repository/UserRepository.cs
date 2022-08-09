using ApplicationCore.Entities;
using ApplicationCore.Contracts.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

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

        public async Task<bool> RemoveFavorite(Favorite favorite)
        {
            var FavToBeRemoved = await _movieShopDbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == favorite.UserId && f.MovieId == favorite.MovieId);
            if (FavToBeRemoved == null)
            {
                throw new Exception("You didn't add this movie to favorite list yet!");
                return false;
            }

            _movieShopDbContext.Favorites.Remove(FavToBeRemoved);
            await _movieShopDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultSet<Movie>> GetAllFavoritesPagination(int userid, int pageSize = 30, int page = 1)
        {
            var totalFavoritesOfUser = await _movieShopDbContext.Favorites.Where(p => p.UserId == userid).CountAsync();
            if (totalFavoritesOfUser == 0)
            {
                //throw new Exception("You didn't add any movie to favorite list yet.");
                return null;
            }

            var movies = await _movieShopDbContext.Favorites.Where(p => p.UserId == userid).Include(p => p.Movie)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalFavoritesOfUser);
            return pagedMovies;
        }

        public async Task<bool> DeleteReview(int userId, int movieId)
        {
            var RevToBeRemoved = await _movieShopDbContext.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
            if (RevToBeRemoved == null)
            {
                throw new Exception("You didn't write a review for this movie yet!");
                return false;
            }

            _movieShopDbContext.Reviews.Remove(RevToBeRemoved);
            await _movieShopDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            var RevToBeUpdated = await _movieShopDbContext.Reviews
                .FirstOrDefaultAsync(r => r.UserId == review.UserId && r.MovieId == review.MovieId);
            if (RevToBeUpdated == null)
            {
                throw new Exception("You didn't write a review for this movie yet!");
                return false;
            }
            RevToBeUpdated.ReviewText = review.ReviewText;
            RevToBeUpdated.Rating = review.Rating;
            RevToBeUpdated.CreatedDate = review.CreatedDate;

            _movieShopDbContext.Reviews.Update(RevToBeUpdated);
            await _movieShopDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Review>> GetAllReviews(int userid)
        {
            var totalReviewsOfUser = await _movieShopDbContext.Reviews.Where(r => r.UserId == userid).CountAsync();
            if (totalReviewsOfUser == 0)
            {
                //throw new Exception("You didn't have any review yet.");
                return null;
            }

            var reviews = await _movieShopDbContext.Reviews.Where(r => r.UserId == userid).ToListAsync();

            return reviews;
        }

        public async Task<bool> EditUserProfile(User editProfile)
        {
            var ProToBeUpdated = await GetById(editProfile.Id);

            if (ProToBeUpdated == null)
            {
                throw new Exception("You didn't register yet!");
                return false;
            }

            ProToBeUpdated.FirstName = editProfile.FirstName;
            ProToBeUpdated.LastName = editProfile.LastName;
            ProToBeUpdated.Email = editProfile.Email;
            ProToBeUpdated.HashedPassword = editProfile.HashedPassword;
            ProToBeUpdated.DateOfBirth = editProfile.DateOfBirth;

            _movieShopDbContext.Users.Update(ProToBeUpdated);
            await _movieShopDbContext.SaveChangesAsync();
            return true;
        }
    }
}