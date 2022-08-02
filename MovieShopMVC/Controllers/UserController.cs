using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using MovieShopMVC.Infra;
using ApplicationCore.Contracts.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;


        public UserController(ICurrentUser currentUser, IUserService userService, IMovieService movieService)
        {
            _currentUser = currentUser;
            _userService = userService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Purchases(int pageSize = 30, int page = 1)
        {
            // get all the movies purchased by user , user id
            // httpcontext.user.claims and then call the database and get the information to the view
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            var userId = _currentUser.UserId;
            var pagedMovies = await _userService.GetAllPurchasesForUser(userId, pageSize, page);
            ViewData["Title"] = "Purchases";
            return View(pagedMovies);
        }

        [HttpGet]
        public async Task<IActionResult> PurchaseDetails(int userId, int movieId)
        {
            var purchaseDetails = await _userService.GetPurchasesDetails(userId, movieId);
            return View(purchaseDetails);
        }


        [HttpGet]
        public async Task<IActionResult> Favorites(int pageSize = 30, int page = 1)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            var userId = _currentUser.UserId;
            var pagedMovies = await _userService.GetAllFavoritesForUser(userId, pageSize, page);
            ViewData["Title"] = "Favorites";
            return View(pagedMovies);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditModel model)
        {

            return View();
        }

        [HttpGet]
        public IActionResult BuyMovie(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }


        [HttpPost]
        public async Task<IActionResult> BuyMovie(int movieId, int userId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }

            UserPurchaseRequestModel purchase = new UserPurchaseRequestModel
            {
                UserId = _currentUser.UserId,
                MovieId = movieId
            };
            var purchaseSuccess = await _userService.PurchaseMovie(purchase, _currentUser.UserId);
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpGet]
        public IActionResult FavoriteMovie(int movieId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpPost]
        public async Task<IActionResult> FavoriteMovie(int movieId, int userId, int actionId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }

            if (actionId == 1)
            {
                UserFavoriteRequestModel favorite = new UserFavoriteRequestModel
                {
                    UserId = _currentUser.UserId,
                    MovieId = movieId
                };
                var favoriteAddSuccess = await _userService.AddFavorite(favorite);
            }
            else if (actionId == 0)
            {
                UserFavoriteRequestModel favorite = new UserFavoriteRequestModel
                {
                    UserId = _currentUser.UserId,
                    MovieId = movieId
                };

                var favoriteRemoveSuccess = await _userService.RemoveFavorite(favorite);
            }

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
        [HttpGet]
        public IActionResult ReviewMovie(int movieId)
        {
            
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            //return RedirectToAction("Details", "Movies", new { id = movieId });
            ViewData["Title"] = "Review" + _movieService.GetMovieDetails(movieId).Result.Title;
            ReviewDetailsModel review = new ReviewDetailsModel
            {
                MovieId = movieId,
                UserId = _currentUser.UserId
            };
            if (_userService.ReviewExists(_currentUser.UserId, movieId).Result)
            {
                review = _userService.GetReviewDetails(_currentUser.UserId, movieId).Result;
            }
          
            return View(review);
        }


        [HttpPost]
        public async Task<IActionResult> ReviewMovie(UserReviewRequestModel model,int movieId ,int actionId)
        {
            if (_currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            UserReviewRequestModel review = new UserReviewRequestModel
            {
                MovieId = movieId,
                UserId = _currentUser.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            };
            

            if (actionId == 1)
            {
                if (await _userService.ReviewExists(_currentUser.UserId, review.MovieId))
                {
                    var reviewUpdateSuccess = await _userService.UpdateMovieReview(review);
                }
                else
                {
                    var reviewAddSuccess = await _userService.AddMovieReview(review);
                }

            }
            else if (actionId == 0)
            {

                var reviewDeleteSuccess = await _userService.DeleteMovieReview(_currentUser.UserId, review.MovieId);
            }



            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }
    }
}
