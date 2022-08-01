using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public UserService(IUserRepository userRepository, IMovieRepository movieRepository, IPurchaseRepository purchaseRepository)
        {
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<bool> AddFavorite(UserFavoriteRequestModel favoriteRequest)
        {
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId) == true)
            {
                throw new Exception("You already add this movie to favorite list!");
            }

            var dbFavorite = new Favorite
            {
                UserId = favoriteRequest.UserId,
                MovieId = favoriteRequest.MovieId
            };
            var savedFavorite = await _userRepository.AddFavorite(dbFavorite);
            if (savedFavorite.UserId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> FavoriteExists(int userid, int movieId)
        {
            var favorite = await _userRepository.GetFavoriteById(userid, movieId);
            if (favorite == null)
            {
                return false;
            }
            return true;
        }

        public async Task<PagedResultSet<MovieCardModel>> GetAllFavoritesForUser(int userid, int pageSize = 30, int page = 1)
        {
            var movies = await _userRepository.GetAllFavoritesPagination(userid, pageSize, page);

            var movieCards = new List<MovieCardModel>();
            movieCards.AddRange(movies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                PosterUrl = m.PosterUrl,
                Title = m.Title
            }));

            return new PagedResultSet<MovieCardModel>(movieCards, page, pageSize, movies.TotalRowCount);
        }

        public async Task<PagedResultSet<MovieCardModel>> GetAllPurchasesForUser(int userid, int pageSize = 30, int page = 1)
        {
            var movies = await _purchaseRepository.GetAllPurchasesPagination(userid, pageSize, page);

            var movieCards = new List<MovieCardModel>();
            movieCards.AddRange(movies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                PosterUrl = m.PosterUrl,
                Title = m.Title
            }));

            return new PagedResultSet<MovieCardModel>(movieCards, page, pageSize, movies.TotalRowCount);
        }

        public async Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId)
        {
            var purchaseDetails = await _userRepository.GetPurchaseById(userId,movieId);
            var purchaseDetailsModel = new PurchaseDetailsModel
            {
                UserId = purchaseDetails.UserId,
                MovieId = purchaseDetails.MovieId,
                PurchaseDateTime = purchaseDetails.PurchaseDateTime,
                PurchaseNumber = purchaseDetails.PurchaseNumber,
                TotalPrice = purchaseDetails.TotalPrice
            };
            return purchaseDetailsModel;
            
        }

        public async Task<bool> IsMoviePurchased(int userId, int movieId)
        {
            var purchase = await _userRepository.GetPurchaseById(userId, movieId);
            if (purchase == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> PurchaseMovie(UserPurchaseRequestModel purchaseRequest, int userId)
        {

            if (await IsMoviePurchased(userId, purchaseRequest.MovieId) == true)
            {
                throw new Exception("You already purchased this movie!");
            }

            var dbPurchase = new Purchase
            {
                UserId = purchaseRequest.UserId,
                PurchaseNumber = purchaseRequest.PurchaseNumber,
                TotalPrice = (decimal)_movieRepository.GetById(purchaseRequest.MovieId).Result.Price,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime,
                MovieId = purchaseRequest.MovieId
            };
            var savedPurchase = await _userRepository.AddPurchase(dbPurchase);
            if (savedPurchase.UserId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveFavorite(UserFavoriteRequestModel favoriteRequest)
        {
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId) == false)
            {
                throw new Exception("You didn't add this movie to favorite list yet!");
            }

            var dbFavorite = new Favorite
            {
                UserId = favoriteRequest.UserId,
                MovieId = favoriteRequest.MovieId
            };

            var removedFavorite = await _userRepository.RemoveFavorite(dbFavorite);
            if (removedFavorite)
            {
                return true;
            }
            return false;
        }
    }
}
