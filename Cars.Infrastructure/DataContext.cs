
using Cars.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;
//using Microsoft.PowerBI.Api.Models;


namespace Cars.Infrastructure
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
    }
}
