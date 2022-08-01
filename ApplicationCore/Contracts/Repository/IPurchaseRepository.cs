using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{

    public interface IPurchaseRepository
    {
        Task<PagedResultSet<Movie>> GetAllPurchasesPagination(int userid, int pageSize = 30, int page = 1);
    }
}
