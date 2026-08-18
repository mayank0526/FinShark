using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IWatchlistRepo
    {
        Task<List<Stock>> GetUserWatchlist(AppUser user);
        Task<Watchlist> CreateAsync(Watchlist watchlist);
        Task<Watchlist> DeleteWatchlist(AppUser appUser, string symbol);
    }
}
