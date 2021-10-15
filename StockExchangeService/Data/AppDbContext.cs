using Microsoft.EntityFrameworkCore;
using StockExchangeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<StockExchange> StockExchanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }

}
