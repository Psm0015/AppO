using AppO.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppO.Data
{
    public class ApplicationDbContext : IdentityDbContext<appUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Follow> Follows { get; set; }
    }
}
