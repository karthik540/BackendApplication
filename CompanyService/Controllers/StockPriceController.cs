using CompanyService.Data;
using CompanyService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockPriceController : ControllerBase
    {
        private readonly AppDbContext context;

        public StockPriceController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> addStockPrice(AddStockPrice model)
        {
            StockPrice newStockPrice = new StockPrice()
            {
                price = model.price,
                dateTime = new DateTime(model.Year, model.Month, model.Day, model.Hour, model.Minute, model.Second),
            };

            var stockExchange = context.StockExchanges.Where(a => a.stockExchange == model.stockExchange).FirstOrDefault();
            var companyStockExchange = context.CompanyStockExchanges.Where(a => a.stockExchangeId == stockExchange.Id && a.stockCode == model.stockCode).FirstOrDefault();

            if (companyStockExchange == null)
            {
                return BadRequest(new
                {
                    message = "Stock Price Creation Failed ! Data Not Found !"
                });
            }

            newStockPrice.companyStockExchange = companyStockExchange;


            try
            {
                await context.StockPrices.AddAsync(newStockPrice);
                context.SaveChanges();
                return Ok(new
                {
                    message = "Stock Price Creation Success !"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Stock Price Creation Failed !"
                });
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> uploadFileHandler(List<UploadStockPrice> models)
        {
            var isValid = true;


            //  Verification of details given...
            foreach (var model in models)
            {
                //  Remove White Space...
                model.CompanyCode = model.CompanyCode.Replace(" ", String.Empty);
                model.StockExchange = model.StockExchange.Replace(" ", String.Empty);
                model.PricePerShare = model.PricePerShare.Replace(" ", String.Empty);
                model.Date = model.Date.Replace(" ", String.Empty);
                model.Time = model.Time.Replace(" ", String.Empty);

                var stockExchange = context.StockExchanges.Where(a => a.stockExchange == model.StockExchange).FirstOrDefault();
                var companyStockExchange = context.CompanyStockExchanges.Where(a => a.stockExchangeId == stockExchange.Id && a.stockCode == model.CompanyCode).FirstOrDefault();
                if (companyStockExchange == null)
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Stock Price Creation Failed !"
                });
            }

            foreach (var model in models)
            {
                AddStockPrice stockPrice = new AddStockPrice()
                {
                    stockExchange = model.StockExchange,
                    stockCode = model.CompanyCode,
                    price = Convert.ToDecimal(model.PricePerShare),
                };


                //  Date Parsing...
                var DateList = model.Date.Split('/', 3);


                if (DateList.Length != 3)
                {
                    return BadRequest(new
                    {
                        message = "Data Invalid !"
                    });
                }

                stockPrice.Day = Convert.ToInt32(DateList[0]);
                stockPrice.Month = Convert.ToInt32(DateList[1]);
                stockPrice.Year = Convert.ToInt32("20" + DateList[2]);


                //  Time Parsing...
                var TimeList = model.Time.Split(':', 3);


                if (TimeList.Length != 3)
                {
                    return BadRequest(new
                    {
                        message = "Data Invalid !"
                    });
                }

                stockPrice.Hour = Convert.ToInt32(TimeList[0]);
                stockPrice.Minute = Convert.ToInt32(TimeList[1]);
                stockPrice.Second = Convert.ToInt32(TimeList[2]);

                await this.addStockPrice(stockPrice);

            }
            return Ok(new
            {
                message = "Stock Price Creation Success !"
            });
        }

        [HttpPost]
        [Route("Company")]
        public async Task<IActionResult> companyStockPriceProvider(IPODetail model)
        {
            try
            {
                var companyStockList = await context.StockPrices.Where(a => a.companyStockExchangecompanyId == model.companyId && a.companyStockExchangestockExchangeId == model.stockExchangeId).OrderBy(x => x.dateTime).Include(a => a.companyStockExchange.company).ToListAsync();

                return Ok(new
                {
                    companyStockList
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    message = "Data Invalid !"
                });
            }
        }

        [HttpPost]
        [Route("Sector")]
        public async Task<IActionResult> sectorStockPriceProvider(SectorStockPriceModal model)
        {
            try
            {
                //var sectorStockPriceList = await context.StockPrices.Include(x => x.companyStockExchange.company.sectorId).Where(x => x.companyStockExchange.company.sectorId == model.sectorId && x.companyStockExchangestockExchangeId == model.stockExchangeId).ToListAsync();
                var sectorStockPriceList = await context.StockPrices.Include(x => x.companyStockExchange.company).Where(x => x.companyStockExchange.company.sectorId == model.sectorId && x.companyStockExchangestockExchangeId == model.stockExchangeId).OrderBy(x => x.dateTime).ToListAsync();
                return Ok(new
                {
                    sectorStockPriceList
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    message = "Data Invalid !"
                });
            }

            
        }
    }
}
