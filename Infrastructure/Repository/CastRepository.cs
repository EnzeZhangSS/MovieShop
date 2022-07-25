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
    public class CastRepository : ICastRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public CastRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }
        public async Task<Cast> GetById(int id)
        {
            var castDetails = await _movieShopDbContext.Casts
                .Include(c => c.MoviesOfCast).ThenInclude(c => c.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);
            return castDetails;
        }
    }
}
