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
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> UM;
        private readonly IStockRepo SR;

        private readonly IPortfolioRepo PR;
        public PortfolioController(UserManager<AppUser> userManager,IStockRepo stockRepo, IPortfolioRepo portfoliorepo )
        {
            UM = userManager;
            SR = stockRepo ;
            PR = portfoliorepo;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetUserPortfolio ()
        {
              var username = User.GetUsername();
              var appUser =  await UM.FindByNameAsync(username);
              var userPortfolio = await PR.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);
            var stock = await  SR.GetBySymbolAsync(symbol);

            if (stock==null)
            return BadRequest("Stock not found");

            var userPortfolio = await PR.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e=> e.Symbol.ToLower()== symbol.ToLower()))
            return BadRequest("Stock alredy in Portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await PR.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500,"Could not create");
            }
            else 
            {
                return Created();
                }
         }

         [HttpDelete]
         [Authorize]

         public async Task <IActionResult> DeletePortfolio(string symbol )
         {

            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);

            var userPortfolio = await PR.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s=> s.Symbol.ToLower()== symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            {
                await PR.DeletePortfolio(appUser,symbol);
            } 
            else
            {
                return BadRequest("Stock not in Portfolio");
            }
                    return Ok();
         }

    }
}
