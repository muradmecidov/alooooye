
using BZLAND.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BZLAND.DAL
{
    public class AddDbContext:IdentityDbContext<IdentityUser>
    {
        public AddDbContext(DbContextOptions<AddDbContext>options):base(options) 
        { 
        
        
        }
        public DbSet<Team> Teams { get; set; }

    }
}
