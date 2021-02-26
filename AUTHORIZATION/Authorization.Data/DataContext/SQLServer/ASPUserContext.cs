using Authorization.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Data.DataContext.SQLServer
{
    //контекст данных
    public class ASPUserContext : IdentityDbContext<User>
    {

        public DbSet<UserProfile> UserProfiles { get; set; }
        
        public ASPUserContext(DbContextOptions<ASPUserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

     }
}
