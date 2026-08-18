using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/watchlist")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly UserManager<AppUser> UM;
        private readonly IStockRepo SR;
        private readonly IWatchlistRepo WR;

        public WatchlistController(UserManager<AppUser> userManager, IStockRepo stockRepo, IWatchlistRepo watchlistRepo)
        {
            UM = userManager;
            SR = stockRepo;
            WR = watchlistRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserWatchlist()
        {
            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);
            var userWatchlist = await WR.GetUserWatchlist(appUser);
            return Ok(userWatchlist);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWatchlist(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);
            var stock = await SR.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return BadRequest("Stock not found");
            }

            var userWatchlist = await WR.GetUserWatchlist(appUser);

            if (userWatchlist.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already in Watchlist");
            }

            var watchlistModel = new Watchlist
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await WR.CreateAsync(watchlistModel);

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteWatchlist(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);
            var userWatchlist = await WR.GetUserWatchlist(appUser);

            var filteredStock = userWatchlist.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            {
                await WR.DeleteWatchlist(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in Watchlist");
            }

            return Ok();
        }
    }
}
