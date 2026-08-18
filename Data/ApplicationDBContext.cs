using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext <AppUser>
    {
        public ApplicationDBContext(
            DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }
 
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> portfolios {get; set; }

        public DbSet<Watchlist> watchlists {get; set; }
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Portfolio>(x=> x.HasKey(p=> new {p.AppUserId, p.StockId}));
 
            builder.Entity<Portfolio>()
            .HasOne(u=> u.AppUser)
            .WithMany(u=> u.portfolios)
            .HasForeignKey(p=> p.AppUserId);
 
            builder.Entity<Portfolio>()
            .HasOne(u=> u.Stock)
            .WithMany(u=> u.portfolios)
            .HasForeignKey(p=> p.StockId);

            builder.Entity<Watchlist>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Watchlist>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.watchlists)
            .HasForeignKey(p => p.AppUserId);

            builder.Entity<Watchlist>()
            .HasOne(u => u.Stock)
            .WithMany(u => u.watchlists)
            .HasForeignKey(p => p.StockId);
 
             
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole {
                    Id = "11111111-1111-1111-1111-111111111111",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                new IdentityRole {
                    Id = "22222222-2222-2222-2222-222222222222",
                    Name = "USER",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}