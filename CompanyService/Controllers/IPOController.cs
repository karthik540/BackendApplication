using CompanyService.Data;
using CompanyService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPOController : ControllerBase
    {

        private readonly AppDbContext context;

        public IPOController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> getIPOList()
        {
            var ipoList = await context.IPODetails.Include(a => a.company).Include(a => a.stockExchange).ToListAsync();

            return Ok(new
            {
                ipoList
            });
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> deleteIPO(IPODetail model)
        {
            try
            {
                var ipoObject = await context.IPODetails.FindAsync(model.Id);
                context.IPODetails.Remove(ipoObject);
                context.SaveChanges();

                return Ok(new
                {
                    message = "IPO Deletion Success !"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = "IPO Deletion Failed !"
                });
            }

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> addIPO(AddIPODetail model)
        {
            IPODetail newIPO = new IPODetail()
            {
                pricePerShare = model.pricePerShare,
                totalShares = model.totalShares,
                openDate = new DateTime(model.Year, model.Month, model.Day, model.Hour, model.Minute, model.Second),
            };

            var company = await context.Companys.FindAsync(model.companyId);
            var stockExchange = await context.StockExchanges.FindAsync(model.stockExchangeId);
            newIPO.company = company;
            newIPO.stockExchange = stockExchange;


            try
            {
                await context.IPODetails.AddAsync(newIPO);
                context.SaveChanges();
                return Ok(new
                {
                    message = "IPO Creation Success !"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "IPO Creation Failed !"
                });
            }
        }
    }
}
