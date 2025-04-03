
using Lamps.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;
//using Microsoft.PowerBI.Api.Models;


namespace Lamps.Infrastructure
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Lamp> Lamps { get; set; }
    }
}
