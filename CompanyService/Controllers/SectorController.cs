using CompanyService.Data;
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
    public class SectorController : ControllerBase
    {

        private readonly AppDbContext context;

        public SectorController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> getSectorList()
        {
            var sectorList = await context.Sectors.ToListAsync();

            return Ok(new
            {
                sectorList
            });
        }
    }
}
