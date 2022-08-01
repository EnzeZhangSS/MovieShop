using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public PurchaseRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }


        public async Task<PagedResultSet<Movie>> GetAllPurchasesPagination(int userid, int pageSize = 30, int page = 1)
        {
            var totalPurchasesOfUser = await _movieShopDbContext.Purchases.Where(p => p.UserId == userid).CountAsync();
            if (totalPurchasesOfUser == 0)
            {
                throw new Exception("You didn't purchase any movie yet.");
            }

            var movies = await _movieShopDbContext.Purchases.Where(p => p.UserId == userid).Include(p => p.Movie).OrderByDescending(p => p.PurchaseDateTime)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalPurchasesOfUser);
            return pagedMovies;
        }
    }
}
