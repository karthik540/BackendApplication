using CompanyService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<StockExchange> StockExchanges { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyStockExchange> CompanyStockExchanges { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<IPODetail> IPODetails { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPrice>().HasKey(sc => new { sc.companyStockExchangecompanyId, sc.companyStockExchangestockExchangeId , sc.dateTime });
            modelBuilder.Entity<CompanyStockExchange>().HasKey(sc => new { sc.companyId, sc.stockExchangeId });

            modelBuilder.Entity<CompanyStockExchange>()
            .HasOne<Company>(sc => sc.company)
            .WithMany(s => s.StockExchangeList)
            .HasForeignKey(sc => sc.companyId);


            modelBuilder.Entity<CompanyStockExchange>()
            .HasOne<StockExchange>(sc => sc.stockExchange)
            .WithMany(s => s.CompanyList)
            .HasForeignKey(sc => sc.stockExchangeId);
        }
    }
}
