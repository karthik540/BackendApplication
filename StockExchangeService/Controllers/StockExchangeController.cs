using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockExchangeService.Data;
using StockExchangeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StockExchangeController : ControllerBase
    {
        private readonly AppDbContext context;

        public StockExchangeController(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets List of Stock Exchange...
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> getStockExchangeList()
        {
            var stockExchangeList = await context.StockExchanges.ToListAsync();

            return Ok(new
            {
                stockExchangeList
            });
        }

        /// <summary>
        /// Add a new Stock Exchange...
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> addStockExchange(AddStockExchangeModel stockExchange)
        {
            StockExchange newStockExchange = new StockExchange()
            {
                stockExchange = stockExchange.stockExchange,
                brief = stockExchange.brief,
                contactAddress = stockExchange.contactAddress,
                remarks = stockExchange.remarks
            };

            try
            {
                await context.StockExchanges.AddAsync(newStockExchange);
                context.SaveChanges();
                return Ok(new
                {
                    message = "Stock Exchange Creation Success !"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Stock Exchange Creation Failed !"
                });
            }
        }
    }
}
