using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Models
{
    public class UdemyIdentityContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public UdemyIdentityContext(DbContextOptions<UdemyIdentityContext> options):base(options) 
        {
        }
    }
}
