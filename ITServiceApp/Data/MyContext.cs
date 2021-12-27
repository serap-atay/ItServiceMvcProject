using ITServiceApp.Models.Entities;
using ITServiceApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITServiceApp.Data
{
    public class MyContext:IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            
        }
        public DbSet<Deneme> Denemeler { get; set; }
    }
}
