using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        public Movie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetTop30HighestRevenueMovies()
        {
            // Go to Database and get the top movies
            // EF Core or Dapper to connect to database

            var movies = new List<Movie>
            {
                          new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
                          new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
                          new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
                          new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
                          new Movie {Id = 5, Title = "Inception", Budget = 1200000},
                          new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
                          new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
                          new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},
                          new Movie{Id = 9, Title = "The Lord of the Rings: The Fellowship of the Ring", Budget = 1200000},
                          new Movie {Id = 10, Title = "The Dark Knight", Budget = 1200000},
                          new Movie {Id = 11, Title = "The Hunger Games", Budget = 1200000},
                          new Movie {Id = 12, Title = "Django Unchained", Budget = 1200000},
                          new Movie{Id = 13, Title = "The Lord of the Rings: The Return of the King", Budget = 1200000},
                          new Movie {Id = 14, Title = "Harry Potter and the Philosopher's Stone", Budget = 1200000},
                          new Movie {Id = 15, Title = "Iron Man", Budget = 1200000},
                          new Movie {Id = 16, Title = "Furious 7", Budget = 1200000}
            };

            return movies;
        }

        public List<Movie> GetTop30RatedMovies()
        {
            throw new NotImplementedException();
        }
    }
}
