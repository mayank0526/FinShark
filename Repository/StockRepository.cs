using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{

    public class StockRepository : IStockRepo
    {
        private readonly ApplicationDBContext con;
        public StockRepository(ApplicationDBContext context)
        {
            con = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await con.Stocks.AddAsync(stockModel);
            await con.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var SM = await con.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (SM == null)
            {
                return null;
            }
            con.Stocks.Remove(SM);
            await con.SaveChangesAsync();
            return SM;

        }

        public async Task<List<Stock>> GetAllAysnc(QueryObject query)
        {
            var stocks = con.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsendding ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsendding ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }
             var skipNumber = (query.PageNumber - 1) * query.PageSize;


            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await con.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await con.Stocks.FirstOrDefaultAsync(s=> s.Symbol == symbol);
        }

        public Task<bool> StockExisit(int id)
        {
            return con.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var ExistingStock = await con.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (ExistingStock == null)
            {
                return null;
            }
            ExistingStock.Symbol = stockDto.Symbol;
            ExistingStock.CompanyName = stockDto.CompanyName;
            ExistingStock.Purchase = stockDto.Purchase;
            ExistingStock.LastDiv = stockDto.LastDiv;
            ExistingStock.Industry = stockDto.Industry;
            ExistingStock.MarketCap = stockDto.MarketCap;

            await con.SaveChangesAsync();

            return ExistingStock;
        }
    }

}