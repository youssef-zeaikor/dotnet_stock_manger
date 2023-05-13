
using Inventory_M.Models.adjustement;
using Inventory_M.Models.Brands;
using Inventory_M.Models.Categories;
using Inventory_M.Models.product;
using Inventory_M.Models.recod;
using Inventory_M.Models.sale;
using Inventory_M.Models.stockin;
using Inventory_M.Models.tradespeople;
using Inventory_M.Models.users_management;
using Inventory_M.Models.vendor;
using Microsoft.EntityFrameworkCore;

namespace Inventory_M.Data
{
    public class InventoryDbContext :  DbContext

    {

        public InventoryDbContext(DbContextOptions options) : base(options)
        {
        }



        public DbSet<user> Users { get; set; }
        public DbSet<role> Roles { get; set; }
        public DbSet<Vendor> Vendor { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public  DbSet<Product> Products { get; set; }
        public DbSet<StockIn> Stockins { get; set; }
        public DbSet<Tradespeople> Tradespeoples { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public DbSet<Adjustement> Adjustements { get; set; }

        public DbSet<Recod> records { get; set; }







        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     modelBuilder.Entity<Sale>()
        //         .Property(p => p.Price)
        //         .HasColumnType("double");

        //     base.OnModelCreating(modelBuilder);
        //     modelBuilder.Entity<Sale>()
        //         .Property(p => p.Disc)
        //         .HasColumnType("double");

        //    ;
        // }




    }
}
