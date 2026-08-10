using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext con;
        private readonly IStockRepo SR;
        public StockController(ApplicationDBContext context, IStockRepo stockRepo)
        {
            SR = stockRepo;
            con = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {

             if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stocks = await SR.GetAllAysnc(query);
            var ss = stocks.Select(s => s.ToStockDto());
            return Ok(stocks);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

             if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var stock = await SR.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {

             if(!ModelState.IsValid)
            return BadRequest(ModelState);


            var stockModel = stockDto.CreateDto();
            await SR.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById),
            new { id = stockModel.Id },
            stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {

             if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var SM = await SR.UpdateAsync(id, updateDto);

            if (SM == null)
            {
                return NotFound();

            }

            return Ok(SM.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
             if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var sm = await SR.DeleteAsync(id);

            if (sm == null)
            {
                return NotFound();
            }
            con.Stocks.Remove(sm);
            con.SaveChanges();

            return NoContent();
        }
    }
}