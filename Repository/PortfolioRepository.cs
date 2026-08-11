using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Models;
using api.Data;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepo
    {
        private readonly ApplicationDBContext con;
        public PortfolioRepository(ApplicationDBContext context)
        {
            con = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
           await con.portfolios.AddAsync(portfolio);
           await con.SaveChangesAsync();
           return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await con.portfolios.FirstOrDefaultAsync(x=> x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

            if(portfolioModel == null)
            {
                return null;
            }
            con.portfolios.Remove(portfolioModel);
            await con.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await  con.portfolios.Where(u=> u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry= stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
           } ).ToListAsync();
        }
    }
}