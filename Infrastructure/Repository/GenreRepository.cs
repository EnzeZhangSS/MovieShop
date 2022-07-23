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
    public class GenreRepository : IGenreRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;

        public GenreRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            var genres = await _movieShopDbContext.Genres.ToListAsync();
            return genres;
        }
    }
}
