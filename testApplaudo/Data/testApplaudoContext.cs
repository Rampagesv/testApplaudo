using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using testApplaudo.Models;

namespace testApplaudo.Models
{
    public class testApplaudoContext : IdentityDbContext<IdentityUser> //DbContext
    {
        public testApplaudoContext(DbContextOptions<testApplaudoContext> options) : base(options)
        {
        }

        //https://www.youtube.com/watch?v=yH4GhmTPf68
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                    new { Id = "0", Name = "Admin", NormalizedName = "ADMIN" },
                    new { Id = "1", Name = "Customer", NormalizedName = "CUSTOMER" }
                    );

            builder.Entity<MovementTypes>().HasData(
                    new { MovementTypeId = 1, MovementName = "Inicial Stock" },
                    new { MovementTypeId = 2, MovementName = "Purchase" },
                    new { MovementTypeId = 3, MovementName = "Restock" },
                    new { MovementTypeId = 4, MovementName = "Inventory Ajusment" }
                    );

            builder.Entity<Products>().HasData(
                    new { ProductId = 1, ProductSKU = "B01M7UII5H", ProductName = "Nespresso by De'Longhi ENV135R Coffee and Espresso Machine, Red", ProductPrice = decimal.Parse("118.56"), inStock = 2 },
                    new { ProductId = 2, ProductSKU = "B018UQ5AMS", ProductName = "Keurig K-Classic Coffee Maker K-Cup Pod, Single Serve, Programmable, Black", ProductPrice = decimal.Parse("79.99"), inStock = 3 },
                    new { ProductId = 3, ProductSKU = "B07SPW37M3", ProductName = "McCafe Decaf Premium Roast Keurig K Cup Coffee Pods 100 Cups", ProductPrice = decimal.Parse("53.17"), inStock = 4 }
                    );

            builder.Entity<ProductLikes>().HasData(
                    new { ProductId = 1, Likes = 2 },
                    new { ProductId = 2, Likes = 4 },
                    new { ProductId = 3, Likes = 6 }
                    );

        }
        public DbSet<testApplaudo.Models.Customer> Customer { get; set; }

        public DbSet<testApplaudo.Models.Products> Products { get; set; }

        public DbSet<testApplaudo.Models.ProductLikes> ProductLikes { get; set; }

        public DbSet<testApplaudo.Models.Purchase> Purchase { get; set; }

        public DbSet<testApplaudo.Models.PurchaseDetails> PurchaseDetails { get; set; }

        public DbSet<testApplaudo.Models.Stock> Stock { get; set; }

        public DbSet<testApplaudo.Models.PriceLog> PriceLog { get; set; }

    }
}
