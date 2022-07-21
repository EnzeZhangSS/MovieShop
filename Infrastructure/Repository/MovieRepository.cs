using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public MovieRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }
        public Movie GetById(int id)
        {
            // select * from movie where id = 1 join genre, cast, moviegerne, moviecast
            var movieDetails = _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .FirstOrDefault(m => m.Id == id);
            return movieDetails;
        }

        public List<Movie> GetTop30HighestRevenueMovies()
        {

            // call the database with EF Core and get the data
            // use MovieShopDbContext and Movies DbSet
            // select top 30 * from Movies order by Revenue
            // corresponding LINQ Query

            var movies = _movieShopDbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToList();
            return movies;
        }

        public List<Movie> GetTop30RatedMovies()
        {
            throw new NotImplementedException();
        }
    }
}
