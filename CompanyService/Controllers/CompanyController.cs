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
    public class CompanyController : ControllerBase
    {

        private readonly AppDbContext context;

        public CompanyController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> getCompanyList()
        {
            //var companyList = context.Companys.Include(a => a.StockExchangeList);


            List<CompanyList> companyList = new List<CompanyList>();

            var companys = context.Companys;

            foreach (var company in companys)
            {
                CompanyList companyObj = new CompanyList()
                {
                    Id = company.Id,
                    company = company.company,
                    turnover = company.turnover,
                    ceo = company.ceo,
                    boardOfDirectors = company.boardOfDirectors,
                    sectorId = company.sectorId,
                    stockCodes = new List<string>(),
                    stockExchanges = new List<StockExchange>(),
                };
                var sector = await context.Sectors.FindAsync(company.sectorId);

                companyObj.sectorName = sector.sector;

                var StockExchangeList = await context.CompanyStockExchanges.Include(a => a.stockExchange).Where(a => a.companyId == company.Id).ToListAsync();


                for (var i = 0; i < StockExchangeList.Count; i++)
                {
                    companyObj.stockExchanges.Add(StockExchangeList[i].stockExchange);
                    companyObj.stockCodes.Add(StockExchangeList[i].stockCode);
                }

                companyList.Add(companyObj);
                //companyList.Add(company);
            }



            return Ok(new
            {
                companyList
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> addCompany(AddCompanyModel model)
        {
            Company newCompany = new Company()
            {
                company = model.company,
                turnover = model.turnover,
                ceo = model.ceo,
                boardOfDirectors = model.boardOfDirectors,
            };

            newCompany.sector = await context.Sectors.FindAsync(model.sector);

            try
            {
                await context.Companys.AddAsync(newCompany);
                context.SaveChanges();

                for (int i = 0; i < model.stockExchanges.Length; i++)
                {
                    CompanyStockExchange companyStockExchange = new CompanyStockExchange()
                    {
                        company = newCompany,
                        stockExchange = await context.StockExchanges.FindAsync(model.stockExchanges[i]),
                        stockCode = model.stockCodes[i]
                    };
                    try
                    {
                        await context.CompanyStockExchanges.AddAsync(companyStockExchange);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new
                        {
                            message = "Company Stock Link Failed !"
                        });
                    }
                }


                return Ok(new
                {
                    message = "Company Creation Success !"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Company Creation Failed !"
                });
            }
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> deleteCompany(CompanyList model)
        {
            try
            {
                var companyObject = await context.Companys.FindAsync(model.Id);
                context.Companys.Remove(companyObject);
                context.SaveChanges();

                return Ok(new
                {
                    message = "Company Deletion Success !"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Company Deletion Failed !"
                });
            }
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> editCompany(AddCompanyModel model)
        {

            try
            {
                var companyObject = await context.Companys.FindAsync(model.id);
                context.Companys.Remove(companyObject);
                context.SaveChanges();

                await this.addCompany(model);
                context.SaveChanges();

                return Ok(new
                {
                    message = "Company Edit Success !"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Company Deletion Failed !"
                });
            }



            return BadRequest(new
            {
                message = "Company Edit Failed !"
            });
        }
    }
}
