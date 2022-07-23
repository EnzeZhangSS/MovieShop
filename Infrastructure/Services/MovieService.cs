using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<MovieDetailsModel> GetMovieDetails(int movieId)
        {
            var movieDetails = await _movieRepository.GetById(movieId);

            var movieDetailsModel = new MovieDetailsModel
            {
                Id = movieDetails.Id,
                Title = movieDetails.Title,
                PosterUrl = movieDetails.PosterUrl,
                BackdropUrl = movieDetails.BackdropUrl,
                OriginalLanguage = movieDetails.OriginalLanguage,
                Overview = movieDetails.Overview,
                Budget = movieDetails.Budget,
                ReleaseDate = movieDetails.ReleaseDate,
                Revenue = movieDetails.Revenue,
                ImdbUrl = movieDetails.ImdbUrl,
                TmdbUrl = movieDetails.TmdbUrl,
                RunTime = movieDetails.RunTime,
                Tagline = movieDetails.Tagline,
                Price = movieDetails.Price
            };

            foreach (var trailer in movieDetails.Trailers)
            {
                movieDetailsModel.Trailers.Add(new TrailerModel
                {
                    Name = trailer.Name,
                    TrailerUrl = trailer.TrailerUrl
                });
            }

            foreach (var cast in movieDetails.CastsOfMovie)
            {
                movieDetailsModel.Casts.Add(new CastModel { Id = cast.CastId, Name = cast.Cast.Name, Character = cast.Character, ProfilePath = cast.Cast.ProfilePath });
            }

            foreach (var genre in movieDetails.GenresOfMovie)
            {
                movieDetailsModel.Genres.Add(new GenreModel { Id = genre.GenreId, Name = genre.Genre.Name });
            }

            return movieDetailsModel;
        }



        public async Task<List<MovieCardModel>> GetTopRevenueMovies()
        {
            var movies = await _movieRepository.GetTop30HighestRevenueMovies();

            var movieCards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardModel { Id = movie.Id, Title = movie.Title, PosterUrl = movie.PosterUrl });
            }
            return movieCards;
        }
    }
}
