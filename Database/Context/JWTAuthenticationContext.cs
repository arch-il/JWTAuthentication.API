using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.API.Database.Context
{
    public class JWTAuthenticationContext : IdentityDbContext<ApplicationUser>
    {
        public JWTAuthenticationContext(DbContextOptions<JWTAuthenticationContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
