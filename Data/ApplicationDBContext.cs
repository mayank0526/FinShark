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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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