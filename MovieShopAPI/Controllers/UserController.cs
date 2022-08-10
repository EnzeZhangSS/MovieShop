using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Contracts.Repository;
using Microsoft.AspNetCore.Authorization;
using MovieShopAPI.Infra;
using ApplicationCore.Models;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(ICurrentUser currentUser, IUserService userService, IUserRepository userRepository)
        {
            _currentUser = currentUser;
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("details/{id:int}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }

            var userDetails = await _userRepository.GetById(id);
            if (userDetails == null)
            {
                //404
                return NotFound(new { errorMessage = "No User Data Found" });
            }

            UserDetailsModel userData = new UserDetailsModel()
            {
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Email = userDetails.Email,
                Id = id,
                DateOfBirth = userDetails.DateOfBirth
            };

            return Ok(userData);
        }


        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser()
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var purchaseDetails = await _userService.GetAllPurchasesForUser(userId, 30, 1);
            if (purchaseDetails == null)
            {
                //404
                return NotFound(new { errorMessage = "No Purchase Found for this User" });
            }

            return Ok(purchaseDetails);
        }

        [HttpGet]
        [Route("purchase-details/{movieId:int}")]
        public async Task<IActionResult> GetPurchaseDetails(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var purchaseDetail = await _userService.GetPurchasesDetails(userId, movieId);
            if (purchaseDetail == null)
            {
                //404
                return NotFound(new { errorMessage = "No Purchase Detial Found" });
            }

            return Ok(purchaseDetail);
        }


        [HttpGet]
        [Route("favorites")]
        public async Task<IActionResult> GetMoviesFavoritedByUser()
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var favoriteDetails = await _userService.GetAllFavoritesForUser(userId, 30, 1);
            if (favoriteDetails == null)
            {
                //404
                return NotFound(new { errorMessage = "No Favorite Found for this User" });
            }

            return Ok(favoriteDetails);
        }

        [HttpGet]
        [Route("movie-reviews")]
        public async Task<IActionResult> GetMoviesReviewsOfUser()
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var reviews = await _userService.GetAllReviewsByUser(_currentUser.UserId);
            if (reviews == null)
            {
                //404
                return NotFound(new { errorMessage = "No Review Found for this User" });
            }

            return Ok(reviews);
        }

        [HttpGet]
        [Route("check-movie-favorite/{movieId:int}")]
        public async Task<IActionResult> CheckMovieFavorite(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var favoriteStatus = await _userService.FavoriteExists(userId, movieId);
            /*
            if (favoriteStatus == null)
            {
                return NotFound(new { errorMessage = "This Movie Doesn't Exist" });
            }
            */
            return Ok(favoriteStatus);
        }

        [HttpGet]
        [Route("check-movie-purchased/{movieId:int}")]
        public async Task<IActionResult> CheckMoviePurchased(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            var userId = _currentUser.UserId;
            var purchaseStatus = await _userService.IsMoviePurchased(userId, movieId);
            /*
            if (purchaseStatus == null)
            {
                return NotFound(new { errorMessage = "This Movie Doesn't Exist" });
            }
            */
            return Ok(purchaseStatus);
        }

        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> PurchaseMovie(int movieId, int userId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }

            UserPurchaseRequestModel purchase = new UserPurchaseRequestModel
            {
                UserId = userId,
                MovieId = movieId
            };
            var purchaseSuccess = await _userService.PurchaseMovie(purchase, _currentUser.UserId);

            if (purchaseSuccess == false)
            {
                return NotFound(new { errorMessage = "Purchase Failed. Please Check Movie ID" });
            }

            var purchaseDetails = await _userService.GetPurchasesDetails(userId, movieId);
            return Ok(purchaseDetails);

        }

        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> FavoriteMovie(int userId, int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }

            UserFavoriteRequestModel favorite = new UserFavoriteRequestModel
            {
                UserId = _currentUser.UserId,
                MovieId = movieId
            };
            var favoriteAddSuccess = await _userService.AddFavorite(favorite);

            if (favoriteAddSuccess == false)
            {
                return NotFound(new { errorMessage = "Favorite Failed" });
            }

            return Ok(favoriteAddSuccess);
        }

        [HttpPost]
        [Route("un-favorite")]
        public async Task<IActionResult> UnFavoriteMovie(int userId, int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }

            UserFavoriteRequestModel favorite = new UserFavoriteRequestModel
            {
                UserId = _currentUser.UserId,
                MovieId = movieId
            };

            var favoriteRemoveSuccess = await _userService.RemoveFavorite(favorite);

            if (favoriteRemoveSuccess == false)
            {
                return NotFound(new { errorMessage = "Un-Favorite Failed" });
            }

            return Ok(favoriteRemoveSuccess);


        }

        [HttpPost]
        [Route("add-review")]
        public async Task<IActionResult> ReviewMovie(UserReviewRequestModel model)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            UserReviewRequestModel review = new UserReviewRequestModel
            {
                MovieId = model.MovieId,
                UserId = _currentUser.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            };


            var reviewAddSuccess = await _userService.AddMovieReview(review);

            if (reviewAddSuccess == false)
            {
                return NotFound(new { errorMessage = "Review Failed" });
            }

            return Ok(reviewAddSuccess);

        }

        [HttpPut]
        [Route("edit-review")]
        public async Task<IActionResult> EditReview(UserReviewRequestModel model)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }
            UserReviewRequestModel review = new UserReviewRequestModel
            {
                MovieId = model.MovieId,
                UserId = _currentUser.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            };


            var reviewEditSuccess = await _userService.UpdateMovieReview(review);

            if (reviewEditSuccess == false)
            {
                return NotFound(new { errorMessage = "Review Edit Failed" });
            }

            return Ok(reviewEditSuccess);

        }


        [HttpDelete]
        [Route("delete-review/{movieId:int}")]
        public async Task<IActionResult> DeleteReview(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return Unauthorized(new { errorMessage = "Not Authorized" });
            }

            var userId = _currentUser.UserId;
            var reviewDeleteSuccess = await _userService.DeleteMovieReview(userId,movieId);

            if (reviewDeleteSuccess == false)
            {
                return NotFound(new { errorMessage = "Review Delete Failed" });
            }

            return Ok(reviewDeleteSuccess);

        }



    }
}
