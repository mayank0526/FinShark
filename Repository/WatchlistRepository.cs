using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class WatchlistRepository : IWatchlistRepo
    {
        private readonly ApplicationDBContext con;

        public WatchlistRepository(ApplicationDBContext context)
        {
            con = context;
        }

        public async Task<Watchlist> CreateAsync(Watchlist watchlist)
        {
            await con.watchlists.AddAsync(watchlist);
            await con.SaveChangesAsync();
            return watchlist;
        }

        public async Task<Watchlist> DeleteWatchlist(AppUser appUser, string symbol)
        {
            var watchlistModel = await con.watchlists.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

            if (watchlistModel == null)
            {
                return null;
            }

            con.watchlists.Remove(watchlistModel);
            await con.SaveChangesAsync();
            return watchlistModel;
        }

        public async Task<List<Stock>> GetUserWatchlist(AppUser user)
        {
            return await con.watchlists.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}
